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
        Dim main_coords As Point = New Point(Rand.Next(1000, ArenaSize.Width - 1000), Rand.Next(1000, ArenaSize.Height - 1000))
        Ships.Add(New Ship(Me) With {.Coo = main_coords}) : Ships(Ships.Count - 1).SetType(main_type, team, True)
        While spawn_allies > 0
            Dim ally_type As String = Nothing
            Select Case Rand.Next(0, 3)
                Case 0
                    ally_type = "Outpost"
                Case 1
                    ally_type = "Defense"
                Case 2
                    ally_type = "Pointvortex"
            End Select
            Dim ally_coords As Point = New Point(main_coords.X + Rand.Next(-600, 600), main_coords.Y + Rand.Next(-600, 600))
            Ships.Add(New Ship(Me) With {.Coo = ally_coords}) : Ships(Ships.Count - 1).SetType(ally_type, team, True)
            spawn_allies -= 1
        End While
    End Sub
    Private Sub InitPlayerTeam()
        Dim power = 100
        Dim origin As PointF
        ' Player Team (0)
        Teams.Add(New Team(Me, AffinityEnum.KIND, ShipstyleEnum.NONE))
        Dim player_team As Team = Teams(Teams.Count - 1)
        ' Player Ships
        If Rand.Next(0, 100) < 75 Then
            SPAWN_STATION_RANDOMLY("Station", player_team, 3)
            Ships(0).UID = MainForm.MAIN_BASE
            origin = Ships(0).Coo
            power -= 25
        Else
            origin = New Point(Rand.Next(1000, ArenaSize.Width - 1000), Rand.Next(1000, ArenaSize.Height - 1000))
        End If
        If Rand.Next(0, 100) < 75 Then
            Ships.Add(New Ship(Me) With {.Coo = New Point(origin.X, origin.Y - 1)})
            Ships(Ships.Count - 1).SetType("Colonizer", player_team, True)
            Ships(Ships.Count - 1).Direction = Helpers.GetQA(Ships(0).Coo.X, Ships(0).Coo.Y, origin.X, origin.Y)
            Ships(Ships.Count - 1).UpsMax += Rand.Next(0, 16)
            power -= 15
        End If
        If Rand.Next(0, 100) < 75 Then
            Ships.Add(New Ship(Me) With {.Coo = New Point(origin.X + 1, origin.Y)})
            Ships(Ships.Count - 1).SetType("Ambassador", player_team, True)
            Ships(Ships.Count - 1).Direction = Helpers.GetQA(Ships(0).Coo.X, Ships(0).Coo.Y, origin.X, origin.Y)
            Ships(Ships.Count - 1).UpsMax += Rand.Next(0, 16)
            power -= 25
        End If
        While power > 0
            Dim types As String() = {"Pusher", "Sacred", "Simpleship", "Artillery", "Bomber", "Dronner", "Scout", "Kastou", "Strange", "MiniColonizer", "Civil_A"}
            Ships.Add(New Ship(Me) With {.Coo = New Point(origin.X - 1, origin.Y)})
            Ships(Ships.Count - 1).SetType(types(Rand.Next(0, types.Length)), player_team, True)
            Ships(Ships.Count - 1).Direction = Helpers.GetQA(Ships(0).Coo.X, Ships(0).Coo.Y, origin.X, origin.Y)
            Ships(Ships.Count - 1).UpsMax += Rand.Next(4, 16)
            power -= 15
        End While
    End Sub
    Sub InitBotsTeams()
        Teams.Add(New Team(Me, AffinityEnum.ALOOF, ShipstyleEnum.NONE))
        boss_team = Teams(Teams.Count - 1)
        'SPAWN_STATION_RANDOMLY("Loneboss", Teams(Teams.Count - 1), 0)
        ' Derelict Asteroids
        For i As Integer = 1 To 85
            Dim T As String = "Asteroide" : If Rand.Next(0, 3) = 0 Then T = "Meteoroide"
            Ships.Add(New Ship(Me) With {.Coo = New Point(Rand.Next(0, ArenaSize.Width), Rand.Next(0, ArenaSize.Width)), .Direction = Rand.Next(0, 360)}) : Ships(Ships.Count - 1).SetType(T, Nothing, True)
        Next
        ' Stars
        For i = 0 To Rand.Next(1, 3)
            Dim T As String = "Star"
            Ships.Add(New Ship(Me) With {.Coo = New Point(Rand.Next(0, ArenaSize.Width), Rand.Next(0, ArenaSize.Width)), .Direction = Rand.Next(0, 360)}) : Ships(Ships.Count - 1).SetType(T, Nothing, True)
        Next
        ' allied NPC
        Teams.Add(New Team(Me, AffinityEnum.KIND, Nothing))
        SPAWN_STATION_RANDOMLY("Station", Teams(Teams.Count - 1), Rand.Next(2, 6))
        ' enemy NPC
        Teams.Add(New Team(Me, AffinityEnum.MEAN, Nothing))
        SPAWN_STATION_RANDOMLY("Station", Teams(Teams.Count - 1), Rand.Next(3, 7))
        ' random NPC Teams AndAlso Ships
        Dim npc_team_count = Rand.Next(4, 10)
        For i = 0 To npc_team_count - 1
            Teams.Add(New Team(Me, Nothing, Nothing))
            SPAWN_STATION_RANDOMLY("Station", Teams(Teams.Count - 1), Rand.Next(4, 8))
        Next
    End Sub


    Sub AntiSuperposition()
        If Ships.Count > 1 Then
            For a As Integer = 0 To Ships.Count - 1
                For b As Integer = 0 To Ships.Count - 1
                    Dim Aship As Ship = Ships(a) : Dim Bship As Ship = Ships(b)
                    If Aship.UID <> Bship.UID Then
                        If Aship.Coo.X + Aship.W > Bship.Coo.X - Bship.W AndAlso Bship.Coo.X + Bship.W > Aship.Coo.X - Aship.W AndAlso Aship.Coo.Y + Aship.W > Bship.Coo.Y - Bship.W AndAlso Bship.Coo.Y + Bship.W > Aship.Coo.Y - Aship.W Then
                            Dim dist As Double = Helpers.GetDistance(Aship.Coo.X, Aship.Coo.Y, Bship.Coo.X, Bship.Coo.Y)
                            If dist < (Aship.W / 2 + Bship.W / 2) Then
                                Dim z As Double = (Aship.W / 2 + Bship.W / 2) - dist
                                Dim QA As Single = Helpers.GetQA(Aship.Coo.X, Aship.Coo.Y, Bship.Coo.X, Bship.Coo.Y)
                                If Bship.SpeedMax <> 0 Then Bship.Coo = Helpers.GetNewPoint(Bship.Coo, QA, z / 4)
                            End If
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
                If Not AShoot.Team Is AShip.Team AndAlso (AShoot.Team Is Nothing OrElse Not AShoot.Team.IsFriendWith(AShip.Team)) Then
                    If Helpers.GetDistance(AShoot.Coo.X, AShoot.Coo.Y, AShip.Coo.X, AShip.Coo.Y) < AShip.W / 2 Then
                        AShoot.Life = 0
                        If AShip.HotDeflector > 0 AndAlso Rand.Next(0, 100) < AShip.HotDeflector Then
                            Effects.Add(New Effect With {.Type = "Deflected2", .Coo = AShoot.Coo, .Direction = AShoot.Direction, .speed = 0})
                        Else
                            If AShip.DeflectorCount > 0 Then
                                Effects.Add(New Effect With {.Type = "Deflected", .Coo = AShoot.Coo, .Direction = AShoot.Direction, .speed = 0})
                            ElseIf AShip.ColdDeflector >= 0 Then
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
                If Ships(i).Life <= 0 Then
                    Effects.Add(New Effect With {.Type = "ExplosionA", .Coo = Ships(i).Coo, .Direction = 0, .Life = 8, .speed = 0})
                    For c As Integer = 1 To Ships(i).W / 8
                        Effects.Add(New Effect With {.Type = "DebrisA", .Coo = Ships(i).Coo, .Direction = Rand.Next(0, 360), .Life = Rand.Next(80, 120), .speed = Rand.Next(3, 7)})
                    Next
                    If Ships(i).Type = "Nuke" Then
                        NuclearEffect = 255
                        For c As Integer = 1 To 256
                            Effects.Add(New Effect With {.Type = "ExplosionA", .Coo = Ships(i).Coo, .Direction = Rand.Next(0, 360), .Life = 8, .speed = Rand.Next(5, 256)})
                        Next
                        Dim FriendlyFireCount As Integer = 0
                        For Each a_ship As Ship In Ships
                            a_ship.Shield = 0
                            a_ship.DeflectorCount = 0
                            a_ship.TakeDamages(8, Nothing)
                            a_ship.TakeDamages(8, Nothing)
                            a_ship.TakeDamages(8, Nothing)
                            a_ship.TakeDamages(8, Nothing)
                            a_ship.TakeDamages(Math.Max(0, Math.Sqrt(Math.Max(0, 7000 - Helpers.GetDistance(Ships(i).Coo.X, Ships(i).Coo.Y, a_ship.Coo.X, a_ship.Coo.Y)))), Nothing)
                            a_ship.TakeDamages(24, Nothing)
                            If Helpers.GetDistance(Ships(i).Coo.X, Ships(i).Coo.Y, a_ship.Coo.X, a_ship.Coo.Y) < 5500 Then
                                a_ship.TakeDamages(10000, Nothing)
                            End If
                            If a_ship.Team.affinity = AffinityEnum.KIND AndAlso a_ship.Life <= 0 AndAlso a_ship.Team.id <> Ships(i).Team.id Then
                                FriendlyFireCount += 1
                            End If
                            If FriendlyFireCount >= 4 Then
                                Ships(i).Team.affinity = AffinityEnum.ALOOF
                                If Ships(i).Team.id = 0 Then
                                    MainForm.WarCriminalLabel.Visible = True
                                End If
                            End If
                        Next
                    End If
                    If Not Ships(i).last_damager_team Is Nothing Then
                        If Ships(i).Type = "Meteoroide" Then
                            Ships(i).last_damager_team.resources.Add(0, 1, 0, 0)
                        ElseIf Ships(i).Type = "Comet" Then
                            Ships(i).last_damager_team.resources.Add(1200, 8, 1, 0)
                        ElseIf Ships(i).Type = "Station" Then
                            Ships(i).last_damager_team.resources.Add(1200, 16, 2, 0)
                        ElseIf Ships(i).Type = "Loneboss" Then
                            Ships(i).last_damager_team.resources.Add(0, 8, 4, 0)
                        ElseIf Ships(i).Type = "Bugs" Then
                            Ships(i).last_damager_team.resources.Add(0, 16, 8, 0)
                        ElseIf Ships(i).Type = "Converter" Then
                            Ships(i).last_damager_team.resources.Add(3200, 12, 6, 0)
                        ElseIf Ships(i).Type = "Purger_Dronner" Then
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
            If aShip.Team Is Nothing OrElse aShip.Team.id <= 1 Then
                Continue For
            End If
            If aShip.Type = "Station" AndAlso Not aShip.Team.IsFriendWith(team) Then
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
            End If
            If Rand.Next(0, 4) = 0 Then
                Type = "Comet"
                Team = boss_team
                Count = 1
            End If
            If Rand.Next(0, 30) = 0 Then
                Type = "Cargo"
                Count = 1
            End If
            If Rand.Next(0, 70) = 0 Then
                Type = "Civil_A"
                Team = AltTeam
                Count = 1
            End If
            If Rand.Next(0, 180) = 0 Then
                Type = "Purger_Dronner"
                Team = AltTeam
                Count = 1
            End If
            If Rand.Next(0, 290) = 0 Then
                Type = "Loneboss"
                Team = AltTeam
                Count = 1
            End If
            If Rand.Next(0, 460) = 0 Then
                Type = "Converter"
                Team = AltTeam
                Count = 1
            End If
            If Rand.Next(0, 1250) = 0 Then
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
                Ships.Add(New Ship(Me) With {.Coo = New Point(Spawn.X + Rand.Next(-50, 50), Spawn.Y + Rand.Next(-50, 50)), .Direction = dir})
                Ships(Ships.Count - 1).SetType(Type, Team, True)
            Next
        End If
    End Sub
    Sub SpawnNPCShips()
        If (ticks Mod 80 = 0) Then
            ' Count Teams's ships
            For Each a_team As Team In Teams
                a_team.ApproxShipCount = 0
            Next
            For Each a_ship As Ship In Ships
                If Not a_ship.Team Is Nothing Then
                    a_ship.Team.ApproxShipCount += 1
                End If
            Next
            ' Summoning / upgrades
            For Each a_ship As Ship In Ships
                If Not a_ship.Team Is Nothing AndAlso ((a_ship.Team.id <> 0 OrElse a_ship.Type = "BomberFactory") AndAlso a_ship.UpProgress = 0 AndAlso a_ship.Team.ApproxShipCount < a_ship.Team.MaxShips) Then
                    Dim wished_upgrade = "Launch_MSL"
                    ' Stations summoning ships
                    If a_ship.Type = "Station" Then
                        If Rand.Next(0, 128) = 0 Then
                            wished_upgrade = "Spawn_Colonizer"
                        ElseIf a_ship.Team.shipstyle = ShipstyleEnum.SIMPLE Then
                            Select Case Rand.Next(0, 6)
                                Case 0
                                    wished_upgrade = "Spawn_Harass"
                                Case 1
                                    wished_upgrade = "Spawn_Hunter"
                                Case Else
                                    wished_upgrade = "Simpleship"
                            End Select
                        ElseIf a_ship.Team.shipstyle = ShipstyleEnum.ADVANCED Then
                            Select Case Rand.Next(0, 3)
                                Case 0
                                    wished_upgrade = "Spawn_Scout"
                                Case 1
                                    wished_upgrade = "Spawn_Artillery"
                                Case 2
                                    wished_upgrade = "Spawn_Bomber"
                            End Select
                        ElseIf a_ship.Team.shipstyle = ShipstyleEnum.DRONES Then
                            Select Case Rand.Next(0, 4)
                                Case 0
                                    wished_upgrade = "Spawn_Dronner"
                                Case 1
                                    wished_upgrade = "Spawn_Drone"
                                Case 2
                                    wished_upgrade = "Spawn_Hunter"
                                Case 3
                                    wished_upgrade = "Spawn_Harass"
                            End Select
                        ElseIf a_ship.Team.shipstyle = ShipstyleEnum.KASTOU Then
                            wished_upgrade = "Spawn_Kastou"
                        ElseIf a_ship.Team.shipstyle = ShipstyleEnum.CRUSHERS Then
                            wished_upgrade = "Spawn_Crusher"
                        ElseIf a_ship.Team.shipstyle = ShipstyleEnum.STRANGE Then
                            Select Case Rand.Next(0, 3)
                                Case 0
                                    wished_upgrade = "Spawn_Strange"
                                Case 1
                                    wished_upgrade = "Spawn_Strange"
                                Case 2
                                    wished_upgrade = "Spawn_Sacred"
                            End Select
                        ElseIf a_ship.Team.shipstyle = ShipstyleEnum.PURGERS Then
                            Select Case Rand.Next(0, 2)
                                Case 0
                                    wished_upgrade = "Summon_Purger_Dronner"
                                Case 1
                                    wished_upgrade = "Spawn_Cargo"
                            End Select
                        ElseIf a_ship.Team.shipstyle = ShipstyleEnum.LEGENDS Then
                            Select Case Rand.Next(0, 7)
                                Case 0
                                    wished_upgrade = "Spawn_MiniColonizer"
                                Case 1
                                    wished_upgrade = "Simpleship"
                                Case 2
                                    wished_upgrade = "Legend_I"
                                Case 3
                                    wished_upgrade = "Legend_K"
                                Case 4
                                    wished_upgrade = "Legend_L"
                                Case 5
                                    wished_upgrade = "Legend_U"
                                Case 6
                                    wished_upgrade = "Legend_Y"
                            End Select
                        End If
                        Upgrade.ForceUpgradeToShip(a_ship, wished_upgrade)
                    End If
                    ' Kastou summoning Yerka
                    If a_ship.Type = "Kastou" Then
                        If Rand.Next(0, 16) < 4 Then Upgrade.ForceUpgradeToShip(a_ship, "Spawn_Yerka")
                    End If
                    ' Crusher jumping
                    If a_ship.Type = "Crusher" Then
                        If Rand.Next(0, 16) < 4 Then Upgrade.ForceUpgradeToShip(a_ship, "Jump_II")
                    End If
                    ' Bugs jumping
                    If a_ship.Type = "Bugs" AndAlso a_ship.ColdDeflector > 25 Then
                        Upgrade.ForceUpgradeToShip(a_ship, "Jump")
                    End If
                    ' Converter jumping when not in combat
                    If a_ship.Type = "Converter" AndAlso (a_ship.Shield >= a_ship.ShieldMax OrElse a_ship.Shield < a_ship.ShieldMax / 8) Then
                        Upgrade.ForceUpgradeToShip(a_ship, "Jump")
                    End If
                    ' Purger jumping
                    If a_ship.Type = "Purger_Dronner" Then
                        If Rand.Next(0, 64) < 6 Then Upgrade.ForceUpgradeToShip(a_ship, "Jump_II")
                    End If
                    ' Dronner summoning drones
                    If a_ship.Type = "Dronner" AndAlso Rand.Next(0, 16) < 4 Then
                        Select Case Rand.Next(0, 3)
                            Case 0
                                wished_upgrade = "Combat_Drone_1"
                            Case 1
                                wished_upgrade = "Combat_Drone_2"
                            Case 2
                                wished_upgrade = "Combat_Drone_3"
                        End Select
                        Upgrade.ForceUpgradeToShip(a_ship, wished_upgrade)
                    End If
                    ' Purger summoning drones
                    If a_ship.Type = "Purger_Dronner" AndAlso Rand.Next(0, 16) < 9 Then
                        Select Case Rand.Next(0, 3)
                            Case 0
                                wished_upgrade = "Purger_Drone_1"
                            Case 1
                                wished_upgrade = "Purger_Drone_2"
                            Case 2
                                wished_upgrade = "Purger_Drone_3"
                        End Select
                        Upgrade.ForceUpgradeToShip(a_ship, wished_upgrade)
                    End If
                    ' Converter summoning alternative converters
                    If a_ship.Type = "Converter" AndAlso Rand.Next(0, 16) < 8 Then
                        Select Case Rand.Next(0, 2)
                            Case 0
                                wished_upgrade = "Spawn_Converter_A"
                            Case 1
                                wished_upgrade = "Spawn_Converter_B"
                        End Select
                        Upgrade.ForceUpgradeToShip(a_ship, wished_upgrade)
                    End If
                    ' Missiles summoning
                    If a_ship.Type = "Loneboss" Then
                        If Rand.Next(0, 16) < 12 Then Upgrade.ForceUpgradeToShip(a_ship, "Launch_MSL_instant")
                    End If
                    If a_ship.Type = "Yerka" Then
                        If Rand.Next(0, 16) < 1 Then Upgrade.ForceUpgradeToShip(a_ship, "Launch_MSL_from_yerka")
                    End If
                    If a_ship.Type = "Scout" Then
                        If Rand.Next(0, 16) < 1 Then Upgrade.ForceUpgradeToShip(a_ship, "Launch_MSL")
                    End If
                    'Upgrading
                    If Rand.Next(0, 8) = 0 AndAlso (a_ship.Type = "Simpleship" OrElse a_ship.Team.upgrade_limit > 0) Then
                        Dim PossibleUps As List(Of Upgrade) = New List(Of Upgrade)
                        For Each AUp As Upgrade In Upgrade.Upgrades
                            Dim ok As Boolean = True
                            Dim Spliter() As String = AUp.Need.Split(" ")
                            If Not a_ship.HaveUp(AUp.Name) AndAlso Not (AUp.Name.StartsWith("Paint")) AndAlso Not (AUp.Name = "Destroy") AndAlso Not (AUp.Name = "Nuke") AndAlso Not (AUp.Name = "Ascend") AndAlso Not (AUp.Name = "Warp") Then
                                For Each ac As String In Spliter
                                    If a_ship.Ups.Count > Math.Min(a_ship.UpsMax, a_ship.Team.upgrade_limit) AndAlso AUp.Install Then
                                        ok = False
                                        Exit For
                                    End If
                                    If a_ship.InterUpgrade(ac, False) = False AndAlso (a_ship.Upgrading Is Nothing OrElse AUp.Name <> a_ship.Upgrading.Name) Then
                                        ok = False
                                        Exit For
                                    End If
                                Next
                                If ok Then
                                    PossibleUps.Add(AUp)
                                End If
                            End If
                        Next
                        If PossibleUps.Count >= 1 Then
                            a_ship.Upgrading = PossibleUps(Rand.Next(0, PossibleUps.Count))
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



    '===' Fonctions '==='
    Public Function GetShipByUID(ByVal UID As String) As Ship
        For Each AShip As Ship In Ships
            If AShip.UID = UID Then
                Return AShip
            End If
        Next
        Return Nothing
    End Function

    '===' UPGRADES SHIPS '==='
    Public Function CountTeamShips(team As Team) As Integer
        Dim count As Integer = 0 : For Each aship As Ship In Ships
            If aship.Team Is team Then
                If aship.UID <> MainForm.MAIN_BASE Then
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