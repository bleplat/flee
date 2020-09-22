Public Class World

    ' Definition
    Public ArenaSize As New Size(20000, 20000)
    Public Seed As Integer
    Public Rand As Random = Nothing

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
        Me.Rand = New Random(Seed)
        InitPlayerTeam()
        InitBotsTeams()
    End Sub

    Sub Tick()
        AutoSpawn()
        AntiSuperposition()
        CheckAll()
        AutoColide()
        AutoUnspawn()
        ticks += 1
    End Sub



    Sub SPAWN_STATION_RANDOMLY(main_type As String, team As Team, spawn_allies As Integer)
        ' main station
        If main_type Is Nothing Then
            main_type = Helpers.RandomStationName(Rand)
        End If
        Dim main_coords As Point = New Point(Rand.Next(1000, ArenaSize.Width - 1000), Rand.Next(1000, ArenaSize.Height - 1000))
        Ships.Add(New Ship(Me, team, main_type) With {.location = main_coords})
        ' turrets
        While spawn_allies > 0
            Dim ally_type As String = Helpers.RandomTurretName(Rand)
            Dim ally_coords As Point = New Point(main_coords.X + Rand.Next(-600, 600), main_coords.Y + Rand.Next(-600, 600))
            Ships.Add(New Ship(Me, team, ally_type) With {.location = ally_coords})
            spawn_allies -= 1
        End While
    End Sub
    Private Sub InitPlayerTeam()
        Dim power = 100
        Dim origin As PointF
        ' Player Team
        Teams.Add(New Team(Me, AffinityEnum.KIND))
        Dim player_team As Team = Teams(Teams.Count - 1)
        player_team.bot_team = False
        ' Player Ships
        If Rand.Next(0, 100) < 85 Then
            SPAWN_STATION_RANDOMLY("Station", player_team, 3)
            origin = Ships(0).location
            power -= 25
        Else
            origin = New Point(Rand.Next(1000, ArenaSize.Width - 1000), Rand.Next(1000, ArenaSize.Height - 1000))
        End If
        If Rand.Next(0, 100) < 75 Then
            Ships.Add(New Ship(Me, player_team, "Colonizer") With {.location = New Point(origin.X, origin.Y - 1)})
            Ships(Ships.Count - 1).direction = Helpers.GetQA(Ships(0).location.X, Ships(0).location.Y, origin.X, origin.Y)
            Ships(Ships.Count - 1).upgrade_slots += Rand.Next(4, 10)
            power -= 15
        End If
        If Rand.Next(0, 100) < 75 Then
            Ships.Add(New Ship(Me, player_team, "Ambassador") With {.location = New Point(origin.X + 1, origin.Y)})
            Ships(Ships.Count - 1).direction = Helpers.GetQA(Ships(0).location.X, Ships(0).location.Y, origin.X, origin.Y)
            Ships(Ships.Count - 1).upgrade_slots += Rand.Next(4, 10)
            power -= 25
        End If
        While power > 0
            Dim types As String() = {"Pusher", "Sacred", "Simpleship", "Artillery", "Bomber", "Dronner", "Scout", "Kastou", "Strange", "MiniColonizer", "Civil_A"}
            Ships.Add(New Ship(Me, player_team, types(Rand.Next(0, types.Length))) With {.location = New Point(origin.X - 1, origin.Y)})
            Ships(Ships.Count - 1).direction = Helpers.GetQA(Ships(0).location.X, Ships(0).location.Y, origin.X, origin.Y)
            Ships(Ships.Count - 1).upgrade_slots += Rand.Next(6, 12)
            power -= 15
        End While
    End Sub
    Sub InitBotsTeams()
        Teams.Add(New Team(Me, AffinityEnum.ALOOF))
        boss_team = Teams(Teams.Count - 1)
        ' Derelict Asteroids
        For i As Integer = 1 To 85
            Dim T As String = "Asteroide" : If Rand.Next(0, 3) = 0 Then T = "Meteoroide"
            Ships.Add(New Ship(Me, Nothing, T) With {.location = New Point(Rand.Next(0, ArenaSize.Width), Rand.Next(0, ArenaSize.Width)), .direction = Rand.Next(0, 360)})
        Next
        ' Stars
        For i = 0 To Rand.Next(1, 3)
            Dim T As String = "Star"
            Ships.Add(New Ship(Me, Nothing, T) With {.location = New Point(Rand.Next(0, ArenaSize.Width), Rand.Next(0, ArenaSize.Width)), .direction = Rand.Next(0, 360)})
        Next
        ' allied NPC
        Teams.Add(New Team(Me, AffinityEnum.KIND))
        SPAWN_STATION_RANDOMLY(Nothing, Teams(Teams.Count - 1), Rand.Next(2, 6))
        ' enemy NPC
        Teams.Add(New Team(Me, AffinityEnum.MEAN))
        SPAWN_STATION_RANDOMLY(Nothing, Teams(Teams.Count - 1), Rand.Next(3, 7))
        ' random NPC Teams and Ships
        Dim npc_team_count = Rand.Next(3, 6)
        For i = 0 To npc_team_count - 1
            Teams.Add(New Team(Me, Nothing))
            SPAWN_STATION_RANDOMLY(Nothing, Teams(Teams.Count - 1), Rand.Next(4, 8))
        Next
    End Sub


    Sub AntiSuperposition()
        If Ships.Count > 1 Then
            For a As Integer = 1 To Ships.Count - 1
                For b As Integer = 0 To a - 1
                    Dim Aship As Ship = Ships(a) : Dim Bship As Ship = Ships(b)
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
            AShip.IA(Rand.Next(0, 10000))
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
                        If AShip.stats.hot_deflector > 0 AndAlso Rand.Next(0, 100) < AShip.stats.hot_deflector Then
                            Effects.Add(New Effect With {.Type = "Deflected2", .Coo = AShoot.Coo, .Direction = AShoot.Direction, .speed = 0})
                        Else
                            If AShip.deflectors_loaded > 0 Then
                                Effects.Add(New Effect With {.Type = "Deflected", .Coo = AShoot.Coo, .Direction = AShoot.Direction, .speed = 0})
                            ElseIf AShip.cold_deflector_charge >= 0 Then
                                Effects.Add(New Effect With {.Type = "Deflected3", .Coo = AShoot.Coo, .Direction = AShoot.Direction, .speed = 0})
                            Else
                                Effects.Add(New Effect With {.Type = "ImpactA", .Coo = AShoot.Coo, .Direction = AShoot.Direction, .speed = 0})
                            End If
                            AShip.TakeDamages(AShoot.Power, AShoot)
                            AShip.last_damager_team = AShoot.Team
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
                    Effects.Add(New Effect With {.Type = "ExplosionA", .Coo = Ships(i).location, .Direction = 0, .Life = 8, .speed = 0})
                    For c As Integer = 1 To Ships(i).stats.width / 8
                        Effects.Add(New Effect With {.Type = "DebrisA", .Coo = Ships(i).location, .Direction = Rand.Next(0, 360), .Life = Rand.Next(80, 120), .speed = Rand.Next(3, 7)})
                    Next
                    If Ships(i).stats.sprite = "Nuke" Then
                        NuclearEffect = 255
                        For c As Integer = 1 To 256
                            Effects.Add(New Effect With {.Type = "ExplosionA", .Coo = Ships(i).location, .Direction = Rand.Next(0, 360), .Life = 8, .speed = Rand.Next(5, 256)})
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
                        If Ships(i).stats.sprite = "Meteoroide" Then
                            Ships(i).last_damager_team.resources.Add(0, 1, 0, 0)
                        ElseIf Ships(i).stats.sprite = "Comet" Then
                            Ships(i).last_damager_team.resources.Add(1200, 8, 1, 0)
                        ElseIf Ships(i).stats.name.Contains("Station") Then
                            Ships(i).last_damager_team.resources.Add(1200, 16, 2, 0)
                        ElseIf Ships(i).stats.sprite = "Loneboss" Then
                            Ships(i).last_damager_team.resources.Add(0, 8, 4, 0)
                        ElseIf Ships(i).stats.sprite = "Bugs" Then
                            Ships(i).last_damager_team.resources.Add(0, 16, 8, 0)
                        ElseIf Ships(i).stats.sprite = "Converter" Then
                            Ships(i).last_damager_team.resources.Add(3200, 12, 6, 0)
                        ElseIf Ships(i).stats.sprite = "Purger_Dronner" Then
                            Ships(i).last_damager_team.resources.Add(1200, 22, 3, 0)
                        End If
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
        If (ticks Mod 90 = 0) Then
            ' Spawn Location
            Dim Spawn As New Point()
            Select Case Rand.Next(0, 4)
                Case 0 'haut
                    Spawn = New Point(Rand.Next(50, ArenaSize.Width - 50), 0)
                Case 1 'bas
                    Spawn = New Point(Rand.Next(50, ArenaSize.Width - 50), ArenaSize.Height)
                Case 2 'gauche
                    Spawn = New Point(0, Rand.Next(50, ArenaSize.Height - 50))
                Case 3 'droite
                    Spawn = New Point(ArenaSize.Width, Rand.Next(50, ArenaSize.Height - 50))
            End Select
            ' Spawn Direction
            Dim dir As Single = 0
            dir = Helpers.GetQA(Spawn.X, Spawn.Y, ArenaSize.Width / 2, ArenaSize.Height / 2) + Rand.Next(-75, 75)
            ' Type AndAlso count
            Dim Type As String = "Asteroide"
            Dim Team As Team = Nothing
            Dim AltTeam = boss_team
            If Rand.Next(0, 3) = 0 Then AltTeam = Teams(Rand.Next(2, Teams.Count))
            Dim Count As Integer = Rand.Next(1, 5)
            If Rand.Next(0, 4) = 0 Then
                Type = "Meteoroide"
                Count = Rand.Next(6, 12)
            ElseIf Rand.Next(0, 5) = 0 Then
                Type = "Comet"
                Team = boss_team
                Count = 1
            ElseIf Rand.Next(0, 30) = 0 Then
                Type = "Cargo"
                Count = 1
            ElseIf Rand.Next(0, 90) = 0 Then
                Type = "Civil_A"
                Team = AltTeam
                Count = 1
            ElseIf Rand.Next(0, 350) = 0 Then
                Type = "Purger_Dronner"
                Team = AltTeam
                Count = 1
            ElseIf Rand.Next(0, 350) = 0 Then
                Type = "Loneboss"
                Team = AltTeam
                Count = 1
            ElseIf Rand.Next(0, 550) = 0 Then
                Type = "Converter"
                Team = AltTeam
                Count = 1
            ElseIf Rand.Next(0, 2000) = 0 Then
                Type = "Bugs"
                Team = AltTeam
                Count = 1
            End If
            If HasTeamWon(Teams(0)) Then
                If Rand.Next(0, 4) = 0 Then
                    Dim possibles = New List(Of String) From {"Loneboss", "Pusher", "Legend_U", "Legend_I", "Legend_L", "Legend_K", "Cargo", "Colonizer", "Converter_A", "Converter_B"}
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
                    Type = possibles(Rand.Next(0, possibles.Count))
                    Team = boss_team
                    Count = 1
                End If
            End If
            For j As Integer = 1 To Count
                Ships.Add(New Ship(Me, Team, Type) With {.location = New Point(Spawn.X + Rand.Next(-50, 50), Spawn.Y + Rand.Next(-50, 50)), .direction = dir})
            Next
        End If
    End Sub

    Sub SpawnNPCShips()
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
                    If Rand.Next(0, 16) < 8 Then
                        ' building ships
                        If a_ship.stats.crafts.Count() > 0 Then
                            wished_upgrade = Helpers.GetRandomSpawnUpgrade(Rand, a_ship)
                            If Not wished_upgrade Is Nothing Then
                                Upgrade.ForceUpgradeToShip(a_ship, wished_upgrade)
                            End If
                        End If
                    ElseIf Rand.Next(0, 8) = 0 Then
                        ' upgrading
                        Dim PossibleUps As List(Of Upgrade) = a_ship.AvailableUpgrades()
                        If PossibleUps.Count >= 1 Then
                            a_ship.Upgrading = PossibleUps(Rand.Next(0, PossibleUps.Count))
                        End If
                    Else
                        ' hardcoded special things
                        ' Crusher jumping
                        If a_ship.stats.name = "Crusher" Then
                            If Rand.Next(0, 16) < 4 Then Upgrade.ForceUpgradeToShip(a_ship, "Jump_II")
                        End If
                        ' Bugs jumping
                        If a_ship.stats.name = "Bugs" AndAlso a_ship.cold_deflector_charge > 25 Then
                            Upgrade.ForceUpgradeToShip(a_ship, "Jump")
                        End If
                        ' Converter jumping when not in combat
                        If a_ship.stats.name = "Converter" AndAlso (a_ship.shield >= a_ship.stats.shield OrElse a_ship.shield < a_ship.stats.shield / 8) Then
                            Upgrade.ForceUpgradeToShip(a_ship, "Jump")
                        End If
                        ' Purger jumping
                        If a_ship.stats.name = "Purger_Dronner" Then
                            If Rand.Next(0, 48) < 4 Then Upgrade.ForceUpgradeToShip(a_ship, "Jump_II")
                        End If
                        ' Missiles summoning
                        If a_ship.stats.name = "Loneboss" Then
                            If Rand.Next(0, 16) < 12 Then Upgrade.ForceUpgradeToShip(a_ship, "Launch_MSL_instant")
                        End If
                        If a_ship.stats.name = "Yerka" Then
                            'If Rand.Next(0, 16) < 1 Then Upgrade.ForceUpgradeToShip(a_ship, "Launch_MSL_from_yerka")
                        End If
                        If a_ship.stats.name = "Scout" Then
                            'If Rand.Next(0, 16) < 1 Then Upgrade.ForceUpgradeToShip(a_ship, "Launch_MSL")
                        End If
                    End If
                End If
            Next
		End If
	End Sub
    Sub AutoSpawn(Optional ByVal force As Boolean = False)
        SpawnDerelictsObjects()
        SpawnNPCShips()
    End Sub

    '===' UPGRADES SHIPS '==='
    Public Function CountTeamShips(team As Team) As Integer
        Dim count As Integer = 0 : For Each aship As Ship In Ships
            If aship.team Is team Then
                If aship.stats.speed <> 0.0 OrElse Not aship.stats.name.Contains("Station") Then
                    count = count + 1
                End If
                If (Not aship.Upgrading Is Nothing) AndAlso aship.Upgrading.Effect.StartsWith("!Sum") Then
                    count = count + 1
                End If
            End If
        Next
        Return count
    End Function


End Class