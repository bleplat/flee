Public Class World

    ' Definition
    Public ArenaSize As New Size(20000, 20000)
    Public Seed As Integer

    ' Randoms
    Public generation_random As Random = Nothing
    Public gameplay_random As Random = Nothing

    ' Content
    Public Teams As New List(Of Team)
    Public Ships As New List(Of Ship)
    Public Shoots As New List(Of Shoot)
    Public Effects As New List(Of Effect)

    ' State
    Public ticks As Integer = 0
    Public NuclearEffect As Integer = 0
    Public boss_team As Team = Nothing

    Sub New(Seed As Integer)
        Me.Seed = Seed
        Me.gameplay_random = New Random(Seed)
        Me.generation_random = New Random(Seed)
        InitTeams()
        InitPlayer()
        InitDerelicts()
        InitBots()

    End Sub

    Sub Tick()
        SpawnDerelictsObjects()
        NPCUpgrades()
        AntiSuperposition()
        CheckAll()
        AutoColide()
        AutoUnspawn()
        ticks += 1
    End Sub

    Sub SPAWN_STATION_RANDOMLY(from_rand As Random, main_type As String, team As Team, spawn_allies As Integer)
        Dim rand As Random = New Random(from_rand.Next())
        ' spawn main station
        If main_type Is Nothing Then
            main_type = Helpers.RandomStationName(rand)
        End If
        Dim main_coords As Point = New Point(rand.Next(1000, ArenaSize.Width - 1000), rand.Next(1000, ArenaSize.Height - 1000))
        For index = 1 To Math.Max(1, Math.Min(8, 2000 / ShipStats.classes(main_type).complexity))
            If index = 1 Then
                Ships.Add(New Ship(Me, team, main_type) With {.location = main_coords})
            Else
                Ships.Add(New Ship(Me, team, main_type) With {.location = New Point(main_coords.X + rand.Next(-512, 513), main_coords.Y + rand.Next(-512, 513))})
            End If
        Next
        ' spawn turrets
        While spawn_allies > 0
            Dim ally_type As String = Helpers.RandomTurretName(rand)
            Dim ally_coords As Point = New Point(main_coords.X + rand.Next(-512, 513), main_coords.Y + rand.Next(-512, 513))
            Ships.Add(New Ship(Me, team, ally_type) With {.location = ally_coords})
            spawn_allies -= 1
        End While
    End Sub
    Private Sub InitTeams()
        Dim rand As New Random(generation_random.Next())
        ' player team
        Teams.Add(New Team(Me, AffinityEnum.KIND))
        ' boss team
        Teams.Add(New Team(Me, AffinityEnum.ALOOF))
        boss_team = Teams(Teams.Count - 1)
        ' 1 allied NPC team
        Teams.Add(New Team(Me, AffinityEnum.KIND))
        ' enemy NPC team
        Teams.Add(New Team(Me, AffinityEnum.MEAN))
        ' random teams
        Dim npc_team_count = rand.Next(3, 6)
        For i = 0 To npc_team_count - 1
            Teams.Add(New Team(Me, Nothing))
        Next
    End Sub
    Private Sub InitPlayer()
        Dim rand As New Random(generation_random.Next())
        Dim power = 100
        Dim origin As PointF
        ' Player Team
        Dim player_team As Team = Teams(0)
        player_team.bot_team = False
        ' Player Ships
        If Seed.ToString().Contains("777") Then
            origin = New Point(rand.Next(1000, ArenaSize.Width - 1000), rand.Next(1000, ArenaSize.Height - 1000))
            Ships.Add(New Ship(Me, player_team, "DeinsCruiser") With {.location = New Point(origin.X, origin.Y - 1)})
            Ships(Ships.Count - 1).direction = Helpers.GetQA(Ships(0).location.X, Ships(0).location.Y, origin.X, origin.Y)
            power -= 35
        ElseIf rand.Next(0, 100) < 85 Then
            SPAWN_STATION_RANDOMLY(rand, "Station", player_team, 3)
            origin = Ships(0).location
            power -= 25
        Else
            origin = New Point(rand.Next(1000, ArenaSize.Width - 1000), rand.Next(1000, ArenaSize.Height - 1000))
        End If
        If rand.Next(0, 100) < 75 Then
            Ships.Add(New Ship(Me, player_team, "Colonizer") With {.location = New Point(origin.X, origin.Y - 1)})
            Ships(Ships.Count - 1).direction = Helpers.GetQA(Ships(0).location.X, Ships(0).location.Y, origin.X, origin.Y)
            Ships(Ships.Count - 1).upgrade_slots += rand.Next(4, 10)
            power -= 15
        End If
        If rand.Next(0, 100) < 75 Then
            Ships.Add(New Ship(Me, player_team, "Ambassador") With {.location = New Point(origin.X + 1, origin.Y)})
            Ships(Ships.Count - 1).direction = Helpers.GetQA(Ships(0).location.X, Ships(0).location.Y, origin.X, origin.Y)
            Ships(Ships.Count - 1).upgrade_slots += rand.Next(4, 10)
            power -= 25
        End If
        While power > 0
            Dim types As String() = {"MiniColonizer", "MiniColonizer", "Artillery", "Bomber", "Scout", "Simpleship", "Pusher", "Hunter"}
            Ships.Add(New Ship(Me, player_team, types(rand.Next(0, types.Length))) With {.location = New Point(origin.X - 1, origin.Y)})
            Ships(Ships.Count - 1).direction = Helpers.GetQA(Ships(0).location.X, Ships(0).location.Y, origin.X, origin.Y)
            Ships(Ships.Count - 1).upgrade_slots += rand.Next(6, 12)
            power -= 15
        End While
    End Sub
    Sub InitDerelicts()
        Dim rand As New Random(generation_random.Next())
        ' Stars
        For i = 0 To rand.Next(1, 3)
            Dim T As String = "Star"
            Ships.Add(New Ship(Me, Nothing, T) With {.location = New Point(rand.Next(0, ArenaSize.Width), rand.Next(0, ArenaSize.Width)), .direction = rand.Next(0, 360)})
        Next
        ' Asteroids
        For i As Integer = 1 To 25
            Dim T As String = "Asteroid"
            Dim location As PointF = New PointF(rand.Next(0, ArenaSize.Width), rand.Next(0, ArenaSize.Height))
            Dim direction As Double = rand.Next(0, 360)
            For j = 0 To rand.Next(0, 4)
                Ships.Add(New Ship(Me, Nothing, T) With {.location = New PointF(location.X + rand.Next(-4, 5), location.Y + rand.Next(-4, 5)), .direction = direction})
            Next
        Next
        ' Meteoroids
        For i As Integer = 1 To 6
            Dim T As String = "Meteoroid"
            Dim location As PointF = New PointF(rand.Next(0, ArenaSize.Width), rand.Next(0, ArenaSize.Height))
            Dim direction As Double = rand.Next(0, 360)
            For j = 0 To rand.Next(4, 12)
                Ships.Add(New Ship(Me, Nothing, T) With {.location = New PointF(location.X + rand.Next(-4, 5), location.Y + rand.Next(-4, 5)), .direction = direction})
            Next
        Next
    End Sub
    Sub InitBots()
        Dim rand As New Random(generation_random.Next())
        For Each team As Team In Teams
            If Not team.affinity = AffinityEnum.ALOOF AndAlso team.bot_team Then
                SPAWN_STATION_RANDOMLY(rand, Nothing, team, rand.Next(6, 12))
            End If
        Next
    End Sub


    Sub AntiSuperposition()
        If Ships.Count > 1 Then
            For a As Integer = 1 To Ships.Count - 1
                Dim Aship As Ship = Ships(a)
                For b As Integer = 0 To a - 1
                    Dim Bship As Ship = Ships(b)
                    If Aship.location.X + Aship.stats.width > Bship.location.X - Bship.stats.width AndAlso Bship.location.X + Bship.stats.width > Aship.location.X - Aship.stats.width AndAlso Aship.location.Y + Aship.stats.width > Bship.location.Y - Bship.stats.width AndAlso Bship.location.Y + Bship.stats.width > Aship.location.Y - Aship.stats.width Then
                        Dim dist As Double = Helpers.Distance(Aship.location, Bship.location)
                        Dim rel_dist As Double = dist - (Aship.stats.width / 2 + Bship.stats.width / 2)
                        If rel_dist < 0 Then
                            Dim z As Double = (-1 * rel_dist / (Aship.stats.width / 2 + Bship.stats.width / 2)) * 0.0125
                            Dim a_to_b As PointF = New PointF(Bship.location.X - Aship.location.X, Bship.location.Y - Aship.location.Y)
                            If Bship.stats.speed <> 0 Then Bship.speed_vec = New PointF(Bship.speed_vec.X + a_to_b.X * z, Bship.speed_vec.Y + a_to_b.Y * z + 0.001)
                            If Aship.stats.speed <> 0 Then Aship.speed_vec = New PointF(Aship.speed_vec.X - a_to_b.X * z, Aship.speed_vec.Y - a_to_b.Y * z)
                        End If
                    End If
                Next
            Next
        End If
    End Sub
    Sub CheckAll()
        '===' Ships '==='
        For i As Integer = Ships.Count - 1 To 0 Step -1
            Ships(i).Check()
        Next
        For Each AShip As Ship In Ships
            AShip.IA(gameplay_random.Next(0, 10000))
        Next
        '===' Shoots '==='
        For i As Integer = Shoots.Count - 1 To 0 Step -1
            Shoots(i).Check()
        Next
        '===' Effets '==='
        For Each AEffect As Effect In Effects
            AEffect.Check()
        Next
    End Sub


    Sub AutoColide()
        For Each AShoot As Shoot In Shoots
            For Each AShip As Ship In Ships
                If Not AShoot.Team Is AShip.team AndAlso (AShoot.Team Is Nothing OrElse Not AShoot.Team.IsFriendWith(AShip.team)) Then
                    If Helpers.Distance(AShoot.Coo.X, AShoot.Coo.Y, AShip.location.X, AShip.location.Y) < AShip.stats.width / 2 Then
                        AShoot.Life = 0
                        If AShip.stats.hot_deflector > 0 AndAlso gameplay_random.Next(0, 100) < AShip.stats.hot_deflector Then
                            Effects.Add(New Effect With {.Type = "EFF_Deflected2", .Coo = AShoot.Coo, .Direction = AShoot.Direction, .speed = 0})
                        Else
                            If AShip.deflectors_loaded > 0 Then
                                Effects.Add(New Effect With {.Type = "EFF_Deflected", .Coo = AShoot.Coo, .Direction = AShoot.Direction, .speed = 0})
                            Else
                                If AShoot.Power < 16 Then
                                    Effects.Add(New Effect With {.Type = "EFF_Impact0", .Coo = AShoot.Coo, .Direction = AShoot.Direction, .speed = 0})
                                ElseIf AShoot.Power < 32 Then
                                    Effects.Add(New Effect With {.Type = "EFF_Impact1", .Coo = AShoot.Coo, .Direction = AShoot.Direction, .speed = 0, .sprite_y = gameplay_random.Next(0, 4)})
                                ElseIf AShoot.Power < 48 Then
                                    Effects.Add(New Effect With {.Type = "EFF_Impact2", .Coo = AShoot.Coo, .Direction = AShoot.Direction, .speed = 0, .sprite_y = gameplay_random.Next(0, 4)})
                                Else
                                    Effects.Add(New Effect With {.Type = "EFF_Impact3", .Coo = AShoot.Coo, .Direction = AShoot.Direction, .speed = 0, .sprite_y = gameplay_random.Next(0, 4)})
                                End If
                            End If
                            AShip.TakeDamages(AShoot.Power, AShoot)
                            AShip.last_damager_team = AShoot.Team
                            If AShip.stats.cold_deflector AndAlso AShip.cold_deflector_charge < AShip.stats.integrity * 4 Then
                                Effects.Add(New Effect With {.Type = "EFF_Deflected3", .Coo = AShoot.Coo, .Direction = AShoot.Direction, .speed = 0})
                            End If
                        End If
                    End If
                End If
            Next
        Next

    End Sub
    Sub AutoUnspawn()
        'Ships
        If Ships.Count > 0 Then
            For i As Integer = Ships.Count - 1 To 0 Step -1
                If Ships(i).integrity <= 0 Then
                    Effects.Add(New Effect With {.Type = "EFF_Destroyed", .Coo = Ships(i).location, .Direction = 0, .Life = 8, .speed = 0})
                    For c As Integer = 1 To Ships(i).stats.width / 8
                        Effects.Add(New Effect With {.Type = "EFF_Debris", .Coo = Ships(i).location, .Direction = gameplay_random.Next(0, 360), .Life = gameplay_random.Next(80, 120), .speed = gameplay_random.Next(3, 7)})
                    Next
                    If Ships(i).weapons.Count > 1 AndAlso (Ships(i).weapons(0).stats.special And Weapon.SpecialBits.SelfNuke) <> 0 Then
                        NuclearEffect = 255
                        For c As Integer = 1 To 256
                            Effects.Add(New Effect With {.Type = "EFF_Destroyed", .Coo = Ships(i).location, .Direction = gameplay_random.Next(0, 360), .Life = 8, .speed = gameplay_random.Next(5, 256)})
                        Next
                        Dim FriendlyFireCount As Integer = 0
                        For Each a_ship As Ship In Ships
                            a_ship.shield = 0
                            a_ship.deflectors_loaded = 0
                            a_ship.TakeDamages(8, Nothing)
                            a_ship.TakeDamages(8, Nothing)
                            a_ship.TakeDamages(8, Nothing)
                            a_ship.TakeDamages(8, Nothing)
                            a_ship.TakeDamages(Math.Max(0, Math.Sqrt(Math.Max(0, 7000 - Helpers.Distance(Ships(i).location.X, Ships(i).location.Y, a_ship.location.X, a_ship.location.Y)))), Nothing)
                            a_ship.TakeDamages(24, Nothing)
                            If Helpers.Distance(Ships(i).location.X, Ships(i).location.Y, a_ship.location.X, a_ship.location.Y) < 5500 Then
                                a_ship.TakeDamages(10000, Nothing)
                            End If
                            If Not a_ship.team Is Nothing AndAlso a_ship.team.affinity = AffinityEnum.KIND AndAlso a_ship.integrity <= 0 AndAlso a_ship.team.id <> Ships(i).team.id Then
                                FriendlyFireCount += 1
                            End If
                            If FriendlyFireCount >= 4 Then
                                Ships(i).team.affinity = AffinityEnum.ALOOF
                                If Ships(i).team.id = 0 Then
                                    MainForm.WarCriminalLabel.Visible = True
                                End If
                            End If
                        Next
                    End If
                    If Not Ships(i).last_damager_team Is Nothing Then
                        Ships(i).last_damager_team.resources.AddLoot(Ships(i).stats.cost)
                    End If
                    Ships.RemoveAt(i)
                End If
            Next
        End If
        'Shoots
        If Shoots.Count > 0 Then
            For i As Integer = Shoots.Count - 1 To 0 Step -1
                If Shoots(i).Life <= 0 Then
                    Shoots.RemoveAt(i)
                End If
            Next
        End If
        'Effects
        If Effects.Count > 0 Then
            For i As Integer = Effects.Count - 1 To 0 Step -1
                If Effects(i).Life <= 0 Then
                    Effects.RemoveAt(i)
                End If
            Next
        End If

    End Sub
    Function HasTeamWon(team As Team) As Boolean
        For Each aShip In Ships
            If aShip.team Is Nothing OrElse aShip.team.id <= 1 Then
                Continue For
            End If
            If aShip.stats.name.Contains("Station") AndAlso Not aShip.team.IsFriendWith(team) Then
                Return False
            End If
        Next
        Return True
    End Function
    Sub SpawnDerelictsObjects()
        Dim rand As Random = generation_random
        If (ticks Mod 128 = 0) Then
            ' Spawn Location
            Dim Spawn As New Point()
            Select Case rand.Next(0, 4)
                Case 0 'haut
                    Spawn = New Point(rand.Next(50, ArenaSize.Width - 50), 0)
                Case 1 'bas
                    Spawn = New Point(rand.Next(50, ArenaSize.Width - 50), ArenaSize.Height)
                Case 2 'gauche
                    Spawn = New Point(0, rand.Next(50, ArenaSize.Height - 50))
                Case 3 'droite
                    Spawn = New Point(ArenaSize.Width, rand.Next(50, ArenaSize.Height - 50))
            End Select
            ' Spawn Direction
            Dim dir As Single = 0
            dir = Helpers.GetQA(Spawn.X, Spawn.Y, ArenaSize.Width / 2, ArenaSize.Height / 2) + rand.Next(-75, 75)
            ' Type AndAlso count
            Dim Type As String = "Asteroid"
            Dim Team As Team = Nothing
            Dim AltTeam = boss_team
            If rand.Next(0, 3) = 0 Then AltTeam = Teams(rand.Next(2, Teams.Count))
            Dim Count As Integer = rand.Next(1, 5)
            If rand.Next(0, 4) = 0 Then
                Type = "Meteoroid"
                Count = rand.Next(6, 12)
            ElseIf rand.Next(0, 5) = 0 Then
                Type = "Comet"
                Count = 1
            ElseIf rand.Next(0, 30) = 0 Then
                Type = "Cargo"
                Count = 1
            ElseIf rand.Next(0, 90) = 0 Then
                Type = "Civil_A"
                Team = AltTeam
                Count = 1
            ElseIf rand.Next(0, 150) = 0 Then
                Type = "Purger_Dronner"
                Team = AltTeam
                Count = 1
            ElseIf rand.Next(0, 175) = 0 Then
                Type = "Loneboss"
                Team = AltTeam
                Count = 1
            ElseIf rand.Next(0, 350) = 0 Then
                Type = "Converter"
                Team = AltTeam
                Count = 1
            ElseIf rand.Next(0, 1250) = 0 Then
                Type = "Bugs"
                Team = AltTeam
                Count = 1
            End If
            If HasTeamWon(Teams(0)) Then
                If rand.Next(0, 2) = 0 Then
                    Dim possibles = New List(Of String) From {"Loneboss", "Civil_A", "Legend_I", "Legend_L", "Legend_K", "Colonizer"}
                    If Teams(0).affinity = AffinityEnum.ALOOF Then
                        possibles.Add("Nuke")
                        possibles.Add("Ambassador")
                    End If
                    If MainForm.has_ascended Then
                        possibles.Add("Ori")
                        possibles.Add("Ori")
                        possibles.Add("Ori")
                        possibles.Add("Ori")
                    End If
                    Type = possibles(rand.Next(0, possibles.Count))
                    Team = boss_team
                    Count = 1
                End If
            End If
            For j As Integer = 1 To Count
                Ships.Add(New Ship(Me, Team, Type) With {.location = New Point(Spawn.X + rand.Next(-4, 5), Spawn.Y + rand.Next(-4, 5)), .direction = dir})
            Next
        End If
    End Sub

    Sub NPCUpgrades()
        Dim rand As Random = generation_random
        If (ticks Mod 80 = 0) Then
            ' Count Teams's ships
            For Each a_team As Team In Teams
                a_team.ship_count_approximation = 0
            Next
            For Each a_ship As Ship In Ships
                If Not a_ship.team Is Nothing Then
                    a_ship.team.ship_count_approximation += 1
                End If
            Next
            ' Summoning / upgrades
            For Each a_ship As Ship In Ships
                If Not a_ship.team Is Nothing AndAlso a_ship.bot_ship AndAlso a_ship.UpProgress = 0 AndAlso a_ship.team.ship_count_approximation < a_ship.team.ship_count_limit Then
                    Dim wished_upgrade = Nothing
                    If rand.Next(0, 16) < 8 Then
                        ' building ships
                        If a_ship.stats.crafts.Count() > 0 Then
                            wished_upgrade = Helpers.GetRandomSpawnUpgrade(rand, a_ship)
                            If Not wished_upgrade Is Nothing Then
                                Upgrade.ForceUpgradeToShip(a_ship, wished_upgrade)
                            End If
                        End If
                    ElseIf rand.Next(0, 2) = 0 Then
                        ' upgrading
                        Dim PossibleUps As List(Of Upgrade) = a_ship.AvailableUpgrades()
                        If PossibleUps.Count >= 1 Then
                            a_ship.Upgrading = PossibleUps(rand.Next(0, PossibleUps.Count))
                        End If
                    Else
                        ' boss bases must suicide to not use the max ship counter
                        If a_ship.team.affinity = AffinityEnum.ALOOF AndAlso a_ship.stats.turn = 0.0 Then
                            Upgrade.ForceUpgradeToShip(a_ship, "Suicide")
                        End If
                        ' ships equiped with cold deflectors jumps when unable to fire
                        If a_ship.cold_deflector_charge > 24 Then
                            If a_ship.CanUpgrade(Upgrade.UpgradeFromName("Jump")) Then
                                Upgrade.ForceUpgradeToShip(a_ship, "Jump")
                            ElseIf a_ship.CanUpgrade(Upgrade.UpgradeFromName("Jump_II")) Then
                                Upgrade.ForceUpgradeToShip(a_ship, "Jump_II")
                            ElseIf a_ship.CanUpgrade(Upgrade.UpgradeFromName("Warp")) Then
                                Upgrade.ForceUpgradeToShip(a_ship, "Warp")
                            End If
                        End If
                        ' ship jumping when not in good shape
                        If (a_ship.integrity + a_ship.shield) < (a_ship.stats.integrity + a_ship.stats.shield) / 3 Then
                            If a_ship.CanUpgrade(Upgrade.UpgradeFromName("Jump")) Then
                                Upgrade.ForceUpgradeToShip(a_ship, "Jump")
                            ElseIf a_ship.CanUpgrade(Upgrade.UpgradeFromName("Jump_II")) Then
                                Upgrade.ForceUpgradeToShip(a_ship, "Jump_II")
                            ElseIf a_ship.CanUpgrade(Upgrade.UpgradeFromName("Warp")) Then
                                Upgrade.ForceUpgradeToShip(a_ship, "Warp")
                            End If
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    '===' UPGRADES SHIPS '==='
    Public Function CountTeamShips(team As Team) As Integer
        Dim count As Integer = 0 : For Each aship As Ship In Ships
            If aship.team Is team Then
                If aship.stats.speed <> 0.0 OrElse Not aship.stats.name.Contains("Station") Then
                    count = count + 1
                End If
                If (Not aship.Upgrading Is Nothing) AndAlso aship.Upgrading.effect.StartsWith("!Sum") Then
                    count = count + 1
                End If
            End If
        Next
        Return count
    End Function


End Class