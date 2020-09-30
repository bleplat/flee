Public Class Upgrade

    ' global
    Public Shared upgrades As New List(Of Upgrade)

    ' identity
    Public name As String = "DefaultUpgradeName"
    Public desc As String = "DefaultUpgradeDesc"
    Public file As String = "Ups"
    Public frame_coords As New Point(0, 0)

    ' condition
    Public not_for_bots As Boolean = False
    Public need As String = ""
    Public cost As MaterialSet = New MaterialSet()

    ' effect
    Public delay As ULong = 0
    Public effect As String = ""
    Public upgrade_slots_requiered As Integer = 1
    Public spawned_ship As String = Nothing

    ' constructor
    Public Sub New() : End Sub
    Public Sub New(name As String)
        Static undercase_separators() As Char = {"_"}
        Me.name = name
        If Me.name.StartsWith("Build_") OrElse Me.name.StartsWith("Launch_") OrElse Me.name.StartsWith("Summon_") OrElse Me.name.StartsWith("Spawn_") Then
            Me.spawned_ship = name.Split(undercase_separators, 2)(1)
            Me.file = ShipStats.classes(Me.spawned_ship).sprite
            Me.cost = ShipStats.classes(Me.spawned_ship).cost
            Me.upgrade_slots_requiered = 0
        End If
    End Sub

    Public Shared Sub LoadRegUpgrades()
        upgrades.Add(New Upgrade With {.name = "Free", .file = "Ups", .frame_coords = New Point(4, 7), .cost = New MaterialSet(), .delay = 10, .need = "", .effect = "!Suicide", .upgrade_slots_requiered = 0, .not_for_bots = True, .desc = "Release this ship's crew."})
        'Peintures
        upgrades.Add(New Upgrade With {.name = "Paint_1", .file = "Ups", .frame_coords = New Point(5, 1), .cost = New MaterialSet(0, 0, 0, 0), .delay = 25, .need = "", .effect = "!C:Lime", .upgrade_slots_requiered = 0, .not_for_bots = True, .desc = "Apply paint."})
        upgrades.Add(New Upgrade With {.name = "Paint_2", .file = "Ups", .frame_coords = New Point(5, 2), .cost = New MaterialSet(0, 0, 0, 0), .delay = 25, .need = "", .effect = "!C:Cyan", .upgrade_slots_requiered = 0, .not_for_bots = True, .desc = "Apply paint."})
        upgrades.Add(New Upgrade With {.name = "Paint_3", .file = "Ups", .frame_coords = New Point(5, 3), .cost = New MaterialSet(0, 0, 0, 0), .delay = 25, .need = "", .effect = "!C:Yellow", .upgrade_slots_requiered = 0, .not_for_bots = True, .desc = "Apply paint."})
        upgrades.Add(New Upgrade With {.name = "Paint_4", .file = "Ups", .frame_coords = New Point(5, 4), .cost = New MaterialSet(0, 0, 0, 0), .delay = 25, .need = "", .effect = "!C:Orange", .upgrade_slots_requiered = 0, .not_for_bots = True, .desc = "Apply paint."})
        upgrades.Add(New Upgrade With {.name = "Paint_5", .file = "Ups", .frame_coords = New Point(5, 5), .cost = New MaterialSet(0, 0, 0, 0), .delay = 25, .need = "", .effect = "!C:Blue", .upgrade_slots_requiered = 0, .not_for_bots = True, .desc = "Apply paint."})
        upgrades.Add(New Upgrade With {.name = "Paint_6", .file = "Ups", .frame_coords = New Point(5, 6), .cost = New MaterialSet(0, 0, 0, 0), .delay = 25, .need = "", .effect = "!C:White", .upgrade_slots_requiered = 0, .not_for_bots = True, .desc = "Apply paint."})
        upgrades.Add(New Upgrade With {.name = "Paint_7", .file = "Ups", .frame_coords = New Point(5, 7), .cost = New MaterialSet(0, 0, 0, 0), .delay = 25, .need = "", .effect = "!C:DimGray", .upgrade_slots_requiered = 0, .not_for_bots = True, .desc = "Apply paint."})

        upgrades.Add(New Upgrade With {.name = "Repair_Armor", .file = "Ups", .frame_coords = New Point(5, 0), .cost = New MaterialSet(200, 0, 0, 0), .delay = 150, .need = "", .effect = "!Fix:10", .upgrade_slots_requiered = 0, .desc = "Help fixing the hull."})
        upgrades.Add(New Upgrade With {.name = "Tardisication", .file = "Ups", .frame_coords = New Point(4, 0), .cost = New MaterialSet(1500, 16, 1, 750), .delay = 50, .need = "", .effect = "!UpsMax:1", .upgrade_slots_requiered = 0, .not_for_bots = True, .desc = "Break physics laws and increase the space inside the ship, allowing more upgrades."})
        upgrades.Add(New Upgrade With {.name = "Fielbots", .file = "Ups", .frame_coords = New Point(5, 0), .cost = New MaterialSet(200, 0, 0, 0), .delay = 50, .need = "?Up:Auto_Nanobots", .effect = "!FixSFull", .upgrade_slots_requiered = 0, .desc = "Help fixing the shield"})

        upgrades.Add(New Upgrade With {.name = "Break_Uranium", .file = "Ups2", .frame_coords = New Point(7, 7), .cost = New MaterialSet(-1200, -8, 1, 0), .delay = 1, .need = "?Type:Station", .effect = "!Fix:1", .upgrade_slots_requiered = 0, .desc = "Sacrifice some Uranium for crystal AndAlso metal."})

        'Special
        upgrades.Add(New Upgrade With {.name = "Build_Nuke", .file = "Nuke", .frame_coords = New Point(0, 0), .cost = New MaterialSet(8000, 32, 8, 2000), .delay = 250, .need = "?Type:Ambassador", .effect = "!Sum:Nuke", .upgrade_slots_requiered = 0, .desc = "When something go wrong, it still can go worse." & vbNewLine & "/!\ Playing with the atom may lead to unexpected results."})
        upgrades.Add(New Upgrade With {.name = "Launch_Comet", .file = "Comet", .frame_coords = New Point(0, 0), .cost = New MaterialSet(2000, 16, 0, 0), .delay = 100, .need = "?Type:Nothing", .effect = "!Sum:Comet", .upgrade_slots_requiered = 0, .desc = "Launch a comet you can control"})
        upgrades.Add(New Upgrade With {.name = "Launch_MSL", .file = "MSL", .frame_coords = New Point(0, 0), .cost = New MaterialSet(1000, 1, 0, 0), .delay = 100, .need = "?Type:Scout", .effect = "!Sum:MSL", .upgrade_slots_requiered = 0, .desc = "Launch a missile you can control"})
        upgrades.Add(New Upgrade With {.name = "Launch_MSL_from_yerka", .file = "MSL", .frame_coords = New Point(0, 0), .cost = New MaterialSet(400, 1, 0, 0), .delay = 100, .need = "?Type:Yerka", .effect = "!Sum:MSL", .upgrade_slots_requiered = 0, .desc = "Launch a missile you can control"})
        upgrades.Add(New Upgrade With {.name = "Launch_2_MSL", .file = "MSL", .frame_coords = New Point(0, 0), .cost = New MaterialSet(1000, 0, 0, 0), .delay = 10, .need = "?Up:Nothing", .effect = "!Sum:MSL !Sum:MSL", .upgrade_slots_requiered = 0, .desc = "Launch a missile you can control"})
        upgrades.Add(New Upgrade With {.name = "Launch_8_MSL", .file = "MSL", .frame_coords = New Point(0, 0), .cost = New MaterialSet(1000, 0, 0, 0), .delay = 100, .need = "?Up:Nothing", .effect = "!Sum:MSL !Sum:MSL !Sum:MSL !Sum:MSL !Sum:MSL !Sum:MSL !Sum:MSL !Sum:MSL", .upgrade_slots_requiered = 0, .desc = "Launch a missile you can control"})
        upgrades.Add(New Upgrade With {.name = "Launch_MSL_instant", .file = "MSL", .frame_coords = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .delay = 0, .need = "?Up:Nothing", .effect = "!Sum:MSL", .upgrade_slots_requiered = 0, .desc = "Launch a missile you can control"})
        upgrades.Add(New Upgrade With {.name = "Ascend", .file = "Ups", .frame_coords = New Point(7, 7), .cost = New MaterialSet(0, 0, 32, 0), .delay = 600, .need = "?Up:Warp_Drive", .effect = "!Ascend %Speed:300 %Life:100 !Regen:16 %Wloadmax:-100 !Shield:600 !Shieldop:100 !C:White %Shieldreg:2500", .upgrade_slots_requiered = 1, .not_for_bots = True, .desc = "Reach the next level of existence."})
        upgrades.Add(New Upgrade With {.name = "Bugs", .file = "Bugs", .frame_coords = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .delay = 100, .need = "?Type:Nothing", .effect = "!Sum:Bugs", .upgrade_slots_requiered = 0, .desc = "What is this thing?"})
        upgrades.Add(New Upgrade With {.name = "Build_Finalizer", .file = "Finalizer", .frame_coords = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .delay = 600, .need = "?Up:Ascend", .effect = "!Sum:Finalizer", .upgrade_slots_requiered = 0, .desc = "Yet, not everybody made it through." & vbNewLine & "We will return for them."})
        upgrades.Add(New Upgrade With {.name = "Auto_Nanobots", .file = "Ups", .frame_coords = New Point(5, 0), .cost = New MaterialSet(0, 0, 0, 0), .delay = 50, .need = "?Type:Nothing", .effect = "%Life:1000 !Regen:1000", .upgrade_slots_requiered = 1, .desc = "Gives lots of hull"})

        'Jump
        upgrades.Add(New Upgrade With {.name = "Jump", .file = "Ups2", .frame_coords = New Point(0, 1), .cost = New MaterialSet(0, 1, 0, 0), .delay = 150, .need = "?Up:Jump_Engine", .effect = "!Jump:58", .upgrade_slots_requiered = 0, .desc = "Jump."})
        upgrades.Add(New Upgrade With {.name = "Jump_II", .file = "Ups2", .frame_coords = New Point(0, 3), .cost = New MaterialSet(0, 1, 0, 0), .delay = 200, .need = "?Up:Jump_Engine_2", .effect = "!Jump:82", .upgrade_slots_requiered = 0, .desc = "Higher Jump, but takes longer to load."})
        upgrades.Add(New Upgrade With {.name = "Warp", .file = "Ups2", .frame_coords = New Point(0, 5), .cost = New MaterialSet(500, 2, 0, 0), .delay = 250, .need = "?Up:Warp_Drive", .effect = "!Teleport:1", .upgrade_slots_requiered = 0, .desc = "Warp anywhere. " & vbNewLine & "This jump takes a while to load."})

        upgrades.Add(New Upgrade With {.name = "BomberFactory", .file = "BomberFactory", .frame_coords = New Point(0, 0), .cost = New MaterialSet(3300, 12, 0, 0), .delay = 300, .need = "?Type:Outpost", .effect = "!Type:BomberFactory", .upgrade_slots_requiered = 1, .desc = "Change into an automated ship factory"})
        '1->2
        upgrades.Add(New Upgrade With {.name = "Improve_To_Scout", .file = "Scout", .frame_coords = New Point(0, 0), .cost = New MaterialSet(1300, 3, 0, 0), .delay = 300, .need = "?Type:Simpleship", .effect = "!Type:Scout", .upgrade_slots_requiered = 1, .desc = "Change into scout ship."})
        upgrades.Add(New Upgrade With {.name = "Improve_To_Bomber", .file = "Bomber", .frame_coords = New Point(0, 0), .cost = New MaterialSet(1600, 3, 0, 0), .delay = 300, .need = "?Type:Simpleship", .effect = "!Type:Bomber", .upgrade_slots_requiered = 1, .desc = "Change into attack ship."})
        upgrades.Add(New Upgrade With {.name = "Improve_To_Artillery", .file = "Artillery", .frame_coords = New Point(0, 0), .cost = New MaterialSet(1600, 3, 0, 0), .delay = 300, .need = "?Type:Simpleship", .effect = "!Type:Artillery", .upgrade_slots_requiered = 1, .desc = "Change into artillery."})
        upgrades.Add(New Upgrade With {.name = "Improve_To_Dronner", .file = "Dronner", .frame_coords = New Point(0, 0), .cost = New MaterialSet(2000, 5, 0, 0), .delay = 300, .need = "?Type:Simpleship", .effect = "!Type:Dronner", .upgrade_slots_requiered = 1, .desc = "Change into ship builder."})

        '2->3
        'Upgrades.Add(New Upgrade With {.Name = "Purger_Dronner", .File = "Purger_Dronner", .PTN = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .CostC = 8, .CostM = 2800, .CostU = 1, .Time = 200, .Need = "?Type:Dronner", .Effect = "!Type:Purger_Dronner", .Install = True, .Desc = "Dronner building different types of drones." & vbNewLine & "/!\ You will not be able to keep building the old ones!"})

        'Deflector
        upgrades.Add(New Upgrade With {.name = "Deflector", .file = "Deflected", .frame_coords = New Point(0, 0), .cost = New MaterialSet(400, 1, 0, 25), .delay = 200, .need = "", .effect = "!Deflector:1", .upgrade_slots_requiered = 1, .desc = "As an alternative to shields, this device block any projectile, but is very slow to reload." & vbNewLine & "Due to the way it works, It is not efficient against fast reloading weapons."})
        upgrades.Add(New Upgrade With {.name = "Deflector_II", .file = "Deflected", .frame_coords = New Point(1, 0), .cost = New MaterialSet(500, 2, 0, 25), .delay = 200, .need = "+Lvl:2" & NPU(), .effect = "!Deflector:1", .upgrade_slots_requiered = 1, .desc = "Block another projectile."})
        upgrades.Add(New Upgrade With {.name = "Deflector_III", .file = "Deflected", .frame_coords = New Point(2, 0), .cost = New MaterialSet(600, 4, 0, 25), .delay = 200, .need = "" & NPU(), .effect = "!Deflector:1", .upgrade_slots_requiered = 1, .desc = "Block another projectile."})
        upgrades.Add(New Upgrade With {.name = "Deflector_IV", .file = "Deflected", .frame_coords = New Point(3, 0), .cost = New MaterialSet(7000, 8, 0, 25), .delay = 200, .need = "" & NPU(), .effect = "!Deflector:1", .upgrade_slots_requiered = 1, .desc = "Block another projectile."})
        upgrades.Add(New Upgrade With {.name = "Hot_Deflector", .file = "Deflected2", .frame_coords = New Point(4, 0), .cost = New MaterialSet(900, 3, 0, 100), .delay = 200, .need = "?Up:Deflector", .effect = "!HotDeflector:8", .upgrade_slots_requiered = 1, .desc = "This unreliable device was create in an attempt to fix the Deflector vulnerabilities."})
        upgrades.Add(New Upgrade With {.name = "Hot_Deflector_II", .file = "Deflected2", .frame_coords = New Point(5, 0), .cost = New MaterialSet(1100, 3, 0, 100), .delay = 200, .need = "+Lvl:2" & NPU(), .effect = "!HotDeflector:8", .upgrade_slots_requiered = 1, .desc = "The Hot Deflector is twice as reliable."})
        upgrades.Add(New Upgrade With {.name = "Cold_Deflector", .file = "Deflected3", .frame_coords = New Point(7, 0), .cost = New MaterialSet(2300, 6, 0, 150), .delay = 200, .need = "+Lvl:2 ?Up:Deflector", .effect = "!ColdDeflector:1", .upgrade_slots_requiered = 1, .desc = "This deflector is able to catch an incredible amount of fire, but prevent your weapons from reloading while it is recovering, AndAlso cause damage to your hull from the inside."})

        'Boucliers
        upgrades.Add(New Upgrade With {.name = "Shield", .file = "Ups", .frame_coords = New Point(3, 0), .cost = New MaterialSet(200, 1, 0, 0), .delay = 100, .need = "", .effect = "!Shield:40 !Shieldop:25", .upgrade_slots_requiered = 1, .desc = "Protection shield blocking 25% of incomming damages."})
        upgrades.Add(New Upgrade With {.name = "Shield_II", .file = "Ups", .frame_coords = New Point(3, 1), .cost = New MaterialSet(400, 1, 0, 0), .delay = 100, .need = "" & NPU(), .effect = "%Shield:10 !Shieldop:25", .upgrade_slots_requiered = 1, .desc = "Shield stop 25% more damages."})
        upgrades.Add(New Upgrade With {.name = "Shield_III", .file = "Ups", .frame_coords = New Point(3, 2), .cost = New MaterialSet(600, 1, 0, 0), .delay = 100, .need = "" & NPU(), .effect = "%Shield:10 !Shieldop:25", .upgrade_slots_requiered = 1, .desc = "Shield stop 25% more damages."})
        upgrades.Add(New Upgrade With {.name = "Shield_IV", .file = "Ups", .frame_coords = New Point(3, 3), .cost = New MaterialSet(800, 2, 0, 50), .delay = 100, .need = "" & NPU(), .effect = "%Shield:10 !Shieldop:25", .upgrade_slots_requiered = 1, .desc = "Shield stop 25% more damages."})
        upgrades.Add(New Upgrade With {.name = "Shield_Capacitor", .file = "Ups", .frame_coords = New Point(3, 4), .cost = New MaterialSet(600, 1, 0, 0), .delay = 100, .need = "?Up:Shield", .effect = "%Shield:50", .upgrade_slots_requiered = 1, .desc = "Shield last 50% longer."})
        upgrades.Add(New Upgrade With {.name = "Shield_Capacitor_II", .file = "Ups", .frame_coords = New Point(3, 5), .cost = New MaterialSet(700, 2, 0, 0), .delay = 100, .need = "" & NPU(), .effect = "%Shield:50", .upgrade_slots_requiered = 1, .desc = "Shield last 50% longer."})
        upgrades.Add(New Upgrade With {.name = "Shield_Loader", .file = "Ups", .frame_coords = New Point(3, 7), .cost = New MaterialSet(800, 1, 0, 100), .delay = 100, .need = "?Up:Shield", .effect = "%Shieldreg:50", .upgrade_slots_requiered = 1, .desc = "Shield recover 50% faster."})

        'Bouclier (autre)
        upgrades.Add(New Upgrade With {.name = "Dynamic_Shield", .file = "Ups2", .frame_coords = New Point(1, 0), .cost = New MaterialSet(700, 2, 0, 0), .delay = 100, .need = "+Lvl:2 ?Up:Shield", .effect = "%Shieldreg:100 %Shield:-50", .upgrade_slots_requiered = 1, .desc = "Shield recover twice as fast, but last half as long."})
        upgrades.Add(New Upgrade With {.name = "Partial_Shield", .file = "Ups2", .frame_coords = New Point(1, 1), .cost = New MaterialSet(700, 2, 0, 0), .delay = 100, .need = "+Lvl:2 ?Up:Shield_II ?Up:Shield_Loader", .effect = "%Shieldreg:75 !Shieldop:-20", .upgrade_slots_requiered = 1, .desc = "Shield recover 75% faster, but let 20% of shoot damages pass."})
        upgrades.Add(New Upgrade With {.name = "Surface_Shield", .file = "Ups2", .frame_coords = New Point(1, 2), .cost = New MaterialSet(700, 2, 0, 0), .delay = 100, .need = "+Lvl:2 ?Up:Shield_II", .effect = "%Shield:75 !Shieldop:-20", .upgrade_slots_requiered = 1, .desc = "Shield last 75% longer, but let 20% of shoot damages pass."})
        upgrades.Add(New Upgrade With {.name = "Alternative_Shield", .file = "Ups2", .frame_coords = New Point(1, 3), .cost = New MaterialSet(700, 3, 0, 100), .delay = 100, .need = "+Lvl:2 ?Up:Shield_Loader", .effect = "%Shieldreg:25 %Shield:25 !Shieldop:-20", .upgrade_slots_requiered = 1, .desc = "Shield recover 25% faster AndAlso last 25% longer, but let 20% of shoot damages pass."})
        upgrades.Add(New Upgrade With {.name = "Bidirectional_Shield", .file = "Ups2", .frame_coords = New Point(1, 4), .cost = New MaterialSet(700, 2, 0, 100), .delay = 100, .need = "+Lvl:1 ?Up:Shield", .effect = "%Shield:25 %Wloadmax:15", .upgrade_slots_requiered = 1, .desc = "Shield last 25% longer, but weapons shoot slower."})
        upgrades.Add(New Upgrade With {.name = "Shield_Capacitor_III", .file = "Ups2", .frame_coords = New Point(3, 2), .cost = New MaterialSet(1200, 3, 1, 0), .delay = 200, .need = "?Up:Shield_Capacitor_II", .effect = "%Shield:75", .upgrade_slots_requiered = 1, .desc = "Shield last 50% longer."})
        upgrades.Add(New Upgrade With {.name = "Shield_Loader_II", .file = "Ups2", .frame_coords = New Point(3, 4), .cost = New MaterialSet(1200, 3, 0, 200), .delay = 200, .need = "?Up:Shield_Loader", .effect = "%Shieldreg:35", .upgrade_slots_requiered = 1, .desc = "Shield recover 35% faster."})
        upgrades.Add(New Upgrade With {.name = "Shield_Patch", .file = "Ups2", .frame_coords = New Point(3, 0), .cost = New MaterialSet(600, 1, 1, 200), .delay = 200, .need = "?Up:Shield_IV", .effect = "!Shieldop:25", .upgrade_slots_requiered = 1, .desc = "Shield stop +25% of the damages (this can fix some of the other upgrades effects)"})

        'Rechagement
        upgrades.Add(New Upgrade With {.name = "Reloader", .file = "Ups", .frame_coords = New Point(2, 0), .cost = New MaterialSet(100, 1, 0, 0), .delay = 100, .need = "+Lvl:1 ?W", .effect = "%Wloadmax:-15", .upgrade_slots_requiered = 1, .desc = "Weapons reload 15% faster."})
        upgrades.Add(New Upgrade With {.name = "Reloader_II", .file = "Ups", .frame_coords = New Point(2, 1), .cost = New MaterialSet(200, 1, 0, 0), .delay = 100, .need = "?W" & NPU(), .effect = "%Wloadmax:-15", .upgrade_slots_requiered = 1, .desc = "Weapons reload 15% faster."})
        upgrades.Add(New Upgrade With {.name = "Reloader_III", .file = "Ups", .frame_coords = New Point(2, 2), .cost = New MaterialSet(300, 1, 0, 0), .delay = 100, .need = "?W" & NPU(), .effect = "%Wloadmax:-15", .upgrade_slots_requiered = 1, .desc = "Weapons reload 15% faster."})
        upgrades.Add(New Upgrade With {.name = "Reloader_IV", .file = "Ups", .frame_coords = New Point(2, 3), .cost = New MaterialSet(400, 1, 0, 0), .delay = 100, .need = "?W" & NPU(), .effect = "%Wloadmax:-15", .upgrade_slots_requiered = 1, .desc = "Weapons reload 15% faster."})
        upgrades.Add(New Upgrade With {.name = "Reloader_V", .file = "Ups", .frame_coords = New Point(2, 4), .cost = New MaterialSet(500, 1, 0, 0), .delay = 100, .need = "?W" & NPU(), .effect = "%Wloadmax:-15", .upgrade_slots_requiered = 1, .desc = "Weapons reload 15% faster."})
        upgrades.Add(New Upgrade With {.name = "Reloader_VI", .file = "Ups", .frame_coords = New Point(2, 5), .cost = New MaterialSet(600, 1, 0, 0), .delay = 100, .need = "?W" & NPU(), .effect = "%Wloadmax:-15", .upgrade_slots_requiered = 1, .desc = "Weapons reload 15% faster."})
        upgrades.Add(New Upgrade With {.name = "Reloader_VII", .file = "Ups", .frame_coords = New Point(2, 6), .cost = New MaterialSet(700, 1, 0, 0), .delay = 100, .need = "?W" & NPU(), .effect = "%Wloadmax:-15", .upgrade_slots_requiered = 1, .desc = "Weapons reload 15% faster."})
        upgrades.Add(New Upgrade With {.name = "Reloader_VIII", .file = "Ups", .frame_coords = New Point(2, 7), .cost = New MaterialSet(800, 1, 0, 0), .delay = 100, .need = "?W" & NPU(), .effect = "%Wloadmax:-15", .upgrade_slots_requiered = 1, .desc = "Weapons reload 15% faster."})

        'Barilet+
        upgrades.Add(New Upgrade With {.name = "Double_Barrel", .file = "Ups", .frame_coords = New Point(7, 0), .cost = New MaterialSet(900, 1, 0, 0), .delay = 100, .need = "?W", .effect = "%Wbar:100 %Wloadmax:80", .upgrade_slots_requiered = 1, .desc = "Loading is 80% longer, but weapons get double shoot." & vbNewLine & "'Two barrilets arround an axis.'"})
        upgrades.Add(New Upgrade With {.name = "Double_Double_Barrel", .file = "Ups", .frame_coords = New Point(7, 5), .cost = New MaterialSet(1200, 1, 0, 0), .delay = 120, .need = "+Lvl:2 ?W" & NPU(), .effect = "%Wbar:100 %Wloadmax:90", .upgrade_slots_requiered = 1, .desc = "Loading is 90% longer, but weapons get double shoot again." & vbNewLine & "'Two groups, of two barrilets arround an axis, arround an axis.'"})
        upgrades.Add(New Upgrade With {.name = "Double_Double_Barrel_Doubled", .file = "Ups", .frame_coords = New Point(7, 6), .cost = New MaterialSet(1400, 1, 0, 0), .delay = 140, .need = "+Lvl:2 ?W" & NPU(), .effect = "%Wbar:100 %Wloadmax:100", .upgrade_slots_requiered = 1, .desc = "Loading is twice as much expensive in time, but they get an awesome improvement." & vbNewLine & "'Two groups, of two barrilets arround an axis, arround an axis. AndAlso itself doubled arround another axis. Trust the engineers.'"})

        'Propultion
        upgrades.Add(New Upgrade With {.name = "Steering", .file = "Ups2", .frame_coords = New Point(2, 0), .cost = New MaterialSet(200, 0, 0, 0), .delay = 100, .need = "?S", .effect = "!Agility:1", .upgrade_slots_requiered = 1, .desc = "Ship turn faster."})
        upgrades.Add(New Upgrade With {.name = "Steering_II", .file = "Ups2", .frame_coords = New Point(2, 1), .cost = New MaterialSet(400, 0, 0, 25), .delay = 150, .need = "?S" & NPU(), .effect = "!Agility:1", .upgrade_slots_requiered = 1, .desc = "Ship turn faster."})
        upgrades.Add(New Upgrade With {.name = "Steering_III", .file = "Ups2", .frame_coords = New Point(2, 2), .cost = New MaterialSet(1200, 2, 0, 50), .delay = 200, .need = "?S" & NPU(), .effect = "!Agility:1", .upgrade_slots_requiered = 1, .desc = "Ship turn faster."})
        upgrades.Add(New Upgrade With {.name = "Engines", .file = "Ups", .frame_coords = New Point(1, 0), .cost = New MaterialSet(200, 1, 0, 0), .delay = 100, .need = "?S", .effect = "%Speed:5", .upgrade_slots_requiered = 1, .desc = "Ship is 5% faster."})
        upgrades.Add(New Upgrade With {.name = "Engines_II", .file = "Ups", .frame_coords = New Point(1, 1), .cost = New MaterialSet(200, 1, 0, 0), .delay = 105, .need = "?S" & NPU(), .effect = "%Speed:10", .upgrade_slots_requiered = 1, .desc = "Ship is 10% faster."})
        upgrades.Add(New Upgrade With {.name = "Engines_III", .file = "Ups", .frame_coords = New Point(1, 2), .cost = New MaterialSet(200, 1, 0, 0), .delay = 110, .need = "?S" & NPU(), .effect = "%Speed:10", .upgrade_slots_requiered = 1, .desc = "Ship is 10% faster."})
        upgrades.Add(New Upgrade With {.name = "Engines_IV", .file = "Ups", .frame_coords = New Point(1, 3), .cost = New MaterialSet(200, 1, 0, 0), .delay = 115, .need = "?S" & NPU(), .effect = "%Speed:5", .upgrade_slots_requiered = 1, .desc = "Ship is 5% faster."})
        upgrades.Add(New Upgrade With {.name = "Engines_V", .file = "Ups", .frame_coords = New Point(1, 4), .cost = New MaterialSet(200, 1, 0, 25), .delay = 120, .need = "?S" & NPU(), .effect = "%Speed:5", .upgrade_slots_requiered = 1, .desc = "Ship is 5% faster."})
        upgrades.Add(New Upgrade With {.name = "Engines_VI", .file = "Ups", .frame_coords = New Point(1, 5), .cost = New MaterialSet(200, 1, 0, 50), .delay = 125, .need = "?S" & NPU(), .effect = "%Speed:10", .upgrade_slots_requiered = 1, .desc = "Ship is 10% faster."})
        upgrades.Add(New Upgrade With {.name = "Engines_VII", .file = "Ups", .frame_coords = New Point(1, 6), .cost = New MaterialSet(200, 1, 0, 75), .delay = 130, .need = "?S" & NPU(), .effect = "%Speed:10", .upgrade_slots_requiered = 1, .desc = "Ship is 10% faster."})
        upgrades.Add(New Upgrade With {.name = "Engines_VIII", .file = "Ups", .frame_coords = New Point(1, 7), .cost = New MaterialSet(200, 1, 0, 100), .delay = 140, .need = "?S" & NPU(), .effect = "%Speed:10", .upgrade_slots_requiered = 1, .desc = "Ship is 10% faster."})

        'Sauts
        upgrades.Add(New Upgrade With {.name = "Jump_Engine", .file = "Ups2", .frame_coords = New Point(0, 0), .cost = New MaterialSet(800, 2, 0, 0), .delay = 200, .need = "?S", .effect = "", .upgrade_slots_requiered = 1, .desc = "Allow the ship to make jumps, at the price of crystals."})
        upgrades.Add(New Upgrade With {.name = "Jump_Engine_2", .file = "Ups2", .frame_coords = New Point(0, 2), .cost = New MaterialSet(1000, 2, 0, 25), .delay = 250, .need = "?S ?Up:Jump_Engine", .effect = "", .upgrade_slots_requiered = 1, .desc = "Allow higher jumps, but is more expensive in crystals."})
        upgrades.Add(New Upgrade With {.name = "Warp_Drive", .file = "Ups2", .frame_coords = New Point(0, 4), .cost = New MaterialSet(1200, 3, 0, 600), .delay = 300, .need = "", .effect = "", .upgrade_slots_requiered = 1, .desc = "Allow you to warp anywhere."})

        'Blindage
        upgrades.Add(New Upgrade With {.name = "Hull", .file = "Ups", .frame_coords = New Point(0, 0), .cost = New MaterialSet(200, 0, 0, 0), .delay = 80, .need = "", .effect = "%Life:5", .upgrade_slots_requiered = 1, .desc = "Increase the hull."})
        upgrades.Add(New Upgrade With {.name = "Hull_II", .file = "Ups", .frame_coords = New Point(0, 1), .cost = New MaterialSet(300, 0, 0, 0), .delay = 90, .need = "" & NPU(), .effect = "%Life:10", .upgrade_slots_requiered = 1, .desc = "Increase the hull."})
        upgrades.Add(New Upgrade With {.name = "Hull_III", .file = "Ups", .frame_coords = New Point(0, 2), .cost = New MaterialSet(600, 0, 0, 0), .delay = 100, .need = "" & NPU(), .effect = "%Life:10", .upgrade_slots_requiered = 1, .desc = "Increase the hull."})
        upgrades.Add(New Upgrade With {.name = "Hull_IV", .file = "Ups", .frame_coords = New Point(0, 3), .cost = New MaterialSet(1300, 0, 0, 0), .delay = 110, .need = "" & NPU(), .effect = "%Life:10", .upgrade_slots_requiered = 1, .desc = "Increase the hull."})
        upgrades.Add(New Upgrade With {.name = "Hull_V", .file = "Ups", .frame_coords = New Point(0, 4), .cost = New MaterialSet(1900, 0, 0, 0), .delay = 120, .need = "" & NPU(), .effect = "%Life:10", .upgrade_slots_requiered = 1, .desc = "Increase the hull."})
        upgrades.Add(New Upgrade With {.name = "Hull_VI", .file = "Ups", .frame_coords = New Point(0, 5), .cost = New MaterialSet(2500, 0, 0, 0), .delay = 130, .need = "" & NPU(), .effect = "%Life:10", .upgrade_slots_requiered = 1, .desc = "Increase the hull."})
        upgrades.Add(New Upgrade With {.name = "Heavy_Hull", .file = "Ups", .frame_coords = New Point(0, 6), .cost = New MaterialSet(3000, 0, 0, 200), .delay = 140, .need = "" & NPU(), .effect = "%Life:15", .upgrade_slots_requiered = 1, .desc = "Increase the hull."})
        upgrades.Add(New Upgrade With {.name = "Heavy_Hull_II", .file = "Ups", .frame_coords = New Point(0, 7), .cost = New MaterialSet(4000, 0, 0, 400), .delay = 150, .need = "" & NPU(), .effect = "%Life:20", .upgrade_slots_requiered = 1, .desc = "Increase the hull, but half the ship speed."})

        'Regen+
        upgrades.Add(New Upgrade With {.name = "Repair_Bots", .file = "Ups", .frame_coords = New Point(7, 1), .cost = New MaterialSet(400, 1, 0, 0), .delay = 100, .need = "+Life:100", .effect = "!Regen:1", .upgrade_slots_requiered = 1, .desc = "Repair the ship faster."})
        upgrades.Add(New Upgrade With {.name = "Repair_Bots_II", .file = "Ups", .frame_coords = New Point(7, 2), .cost = New MaterialSet(700, 1, 0, 0), .delay = 120, .need = "+Life:200" & NPU(), .effect = "!Regen:1", .upgrade_slots_requiered = 1, .desc = "Repair the ship faster."})
        upgrades.Add(New Upgrade With {.name = "Repair_Bots_III", .file = "Ups", .frame_coords = New Point(7, 3), .cost = New MaterialSet(1400, 1, 0, 0), .delay = 130, .need = "+Life:300" & NPU(), .effect = "!Regen:1", .upgrade_slots_requiered = 1, .desc = "Repair the ship faster."})
        upgrades.Add(New Upgrade With {.name = "Repair_Bots_IV", .file = "Ups", .frame_coords = New Point(7, 4), .cost = New MaterialSet(2000, 2, 0, 0), .delay = 150, .need = "+Life:400" & NPU(), .effect = "!Regen:1", .upgrade_slots_requiered = 1, .desc = "Repair the ship faster."})

        'BASE radio
        upgrades.Add(New Upgrade With {.name = "Multiunit_Control", .file = "Ups", .frame_coords = New Point(6, 0), .cost = New MaterialSet(1000, 1, 0, 0), .delay = 100, .need = "?Base", .effect = "!Maxships:2", .upgrade_slots_requiered = 1, .desc = "Control 2 more ships"})
        upgrades.Add(New Upgrade With {.name = "Multiunit_Control_II", .file = "Ups", .frame_coords = New Point(6, 1), .cost = New MaterialSet(2000, 2, 0, 0), .delay = 150, .need = "?Base" & NPU(), .effect = "!Maxships:3", .upgrade_slots_requiered = 1, .desc = "Control 3 more ships"})
        upgrades.Add(New Upgrade With {.name = "Multiunit_Control_III", .file = "Ups", .frame_coords = New Point(6, 2), .cost = New MaterialSet(3000, 3, 0, 0), .delay = 200, .need = "?Base" & NPU(), .effect = "!Maxships:2", .upgrade_slots_requiered = 1, .desc = "Control 2 more ships"})
        upgrades.Add(New Upgrade With {.name = "Multiunit_Control_IV", .file = "Ups", .frame_coords = New Point(6, 3), .cost = New MaterialSet(4000, 4, 0, 0), .delay = 200, .need = "?Base" & NPU(), .effect = "!Maxships:3", .upgrade_slots_requiered = 1, .desc = "Control 3 more ships"})
        upgrades.Add(New Upgrade With {.name = "Multiunit_Control_V", .file = "Ups", .frame_coords = New Point(6, 4), .cost = New MaterialSet(5000, 5, 0, 0), .delay = 250, .need = "?Base" & NPU(), .effect = "!Maxships:2", .upgrade_slots_requiered = 1, .desc = "Control 2 more ships"})
        upgrades.Add(New Upgrade With {.name = "Multiunit_Control_VI", .file = "Ups", .frame_coords = New Point(6, 5), .cost = New MaterialSet(6000, 6, 0, 0), .delay = 250, .need = "?Base" & NPU(), .effect = "!Maxships:3", .upgrade_slots_requiered = 1, .desc = "Control 3 more ships"})
        upgrades.Add(New Upgrade With {.name = "Multiunit_Control_VII", .file = "Ups", .frame_coords = New Point(6, 6), .cost = New MaterialSet(7000, 7, 0, 100), .delay = 250, .need = "?Base" & NPU(), .effect = "!Maxships:2", .upgrade_slots_requiered = 1, .desc = "Control 2 more ships"})
        upgrades.Add(New Upgrade With {.name = "Multiunit_Control_VIII", .file = "Ups", .frame_coords = New Point(6, 7), .cost = New MaterialSet(8000, 8, 0, 200), .delay = 300, .need = "?Base" & NPU(), .effect = "!Maxships:2", .upgrade_slots_requiered = 1, .desc = "Control 2 more ships"})

        'BASE ups+
        upgrades.Add(New Upgrade With {.name = "Space_Shemes", .file = "Ups", .frame_coords = New Point(4, 0), .cost = New MaterialSet(1000, 1, 0, 0), .delay = 100, .need = "?Base", .effect = "!Upsbonus:2", .upgrade_slots_requiered = 1, .desc = "New ships can install 2 additional upgrades."})
        upgrades.Add(New Upgrade With {.name = "Space_Shemes_II", .file = "Ups", .frame_coords = New Point(4, 1), .cost = New MaterialSet(2000, 2, 0, 0), .delay = 100, .need = "?Base" & NPU(), .effect = "!Upsbonus:2", .upgrade_slots_requiered = 1, .desc = "New ships can install 2 additional upgrades."})
        upgrades.Add(New Upgrade With {.name = "Space_Shemes_III", .file = "Ups", .frame_coords = New Point(4, 2), .cost = New MaterialSet(3000, 4, 0, 0), .delay = 100, .need = "?Base" & NPU(), .effect = "!Upsbonus:2", .upgrade_slots_requiered = 1, .desc = "New ships can install 2 additional upgrades."})
        upgrades.Add(New Upgrade With {.name = "Space_Shemes_IV", .file = "Ups", .frame_coords = New Point(4, 4), .cost = New MaterialSet(4000, 8, 0, 0), .delay = 100, .need = "?Base" & NPU(), .effect = "!Upsbonus:2", .upgrade_slots_requiered = 1, .desc = "New ships can install 2 additional upgrades."})
        upgrades.Add(New Upgrade With {.name = "Space_Shemes_V", .file = "Ups", .frame_coords = New Point(4, 3), .cost = New MaterialSet(5000, 16, 0, 0), .delay = 100, .need = "?Base" & NPU(), .effect = "!Upsbonus:2", .upgrade_slots_requiered = 1, .desc = "New ships can install 2 additional upgrades."})

        'BASE Spawn
        upgrades.Add(New Upgrade With {.name = "Conceive_Legend_I", .file = "Legend_I", .frame_coords = New Point(0, 0), .cost = New MaterialSet(1400, 16, 1, 0), .delay = 650, .need = "?MS ?Type:Station", .effect = "!Sum:Legend_I", .upgrade_slots_requiered = 1, .desc = "Build a legendary ship." & vbNewLine & "/!\ You can only build this ship once!"})
        upgrades.Add(New Upgrade With {.name = "Conceive_Legend_K", .file = "Legend_K", .frame_coords = New Point(0, 0), .cost = New MaterialSet(1200, 14, 0, 400), .delay = 650, .need = "?MS ?Type:Station", .effect = "!Sum:Legend_K", .upgrade_slots_requiered = 1, .desc = "Build a legendary ship." & vbNewLine & "/!\ You can only build this ship once!"})
        upgrades.Add(New Upgrade With {.name = "Conceive_Legend_L", .file = "Legend_L", .frame_coords = New Point(0, 0), .cost = New MaterialSet(1300, 15, 0, 400), .delay = 650, .need = "?MS ?Type:Station", .effect = "!Sum:Legend_L", .upgrade_slots_requiered = 1, .desc = "Build a legendary ship." & vbNewLine & "/!\ You can only build this ship once!"})
        upgrades.Add(New Upgrade With {.name = "Conceive_Legend_U", .file = "Legend_U", .frame_coords = New Point(0, 0), .cost = New MaterialSet(1200, 16, 0, 200), .delay = 650, .need = "?MS ?Type:Station", .effect = "!Sum:Legend_U", .upgrade_slots_requiered = 1, .desc = "Build a legendary ship." & vbNewLine & "/!\ You can only build this ship once!"})
        upgrades.Add(New Upgrade With {.name = "Conceive_Legend_Y", .file = "Legend_Y", .frame_coords = New Point(0, 0), .cost = New MaterialSet(1400, 14, 0, 300), .delay = 650, .need = "?MS ?Type:Station", .effect = "!Sum:Legend_Y", .upgrade_slots_requiered = 1, .desc = "Build a legendary ship." & vbNewLine & "/!\ You can only build this ship once!"})

        'NPC Spawn

        'Drone Spawn

        'Purger Drones
        upgrades.Add(New Upgrade With {.name = "Purger_Control", .file = "Ups", .frame_coords = New Point(6, 7), .cost = New MaterialSet(32000, 32, 0, 0), .delay = 300, .need = "?Type:Purger_Dronner", .effect = "!Maxships:8", .upgrade_slots_requiered = 1, .desc = "Control 8 more ships"})

        'Bases

        upgrades.Add(New Upgrade With {.name = "Build_PartialBomber", .file = "PartialBomber", .frame_coords = New Point(0, 0), .cost = New MaterialSet(600, 0, 0, 25), .delay = 2700, .need = "?MS ?Type:BomberFactory", .effect = "!Sum:PartialBomber", .upgrade_slots_requiered = 0, .desc = ""})
    End Sub
    Public Shared Sub LoadBuildUpgrades()
        For Each ship_class As ShipStats In ShipStats.classes.Values
            Dim build_ship_upgrade_name As String = "Build_" & ship_class.name
            Dim launch_ship_upgrade_name As String = "Launch_" & ship_class.name
            If UpgradeFromName(build_ship_upgrade_name) Is Nothing AndAlso UpgradeFromName(launch_ship_upgrade_name) Is Nothing Then
                upgrades.Add(New Upgrade(build_ship_upgrade_name) With {.cost = ship_class.cost, .delay = ship_class.complexity, .need = "?MS", .effect = "!Sum:" & ship_class.name, .upgrade_slots_requiered = False, .desc = "Build a " & ship_class.name & "."})
            End If
        Next
    End Sub

    Public Shared Function NPU() As String
        Return " ?Up:" & upgrades(upgrades.Count - 1).name
    End Function

    Public Shared Function UpgradeFromName(up_name As String) As Upgrade
        For Each aUp In upgrades
            If aUp.name = up_name Then
                Return aUp
            End If
        Next
        Return Nothing
    End Function
    Public Shared Sub ForceUpgradeToShip(a_ship As Ship, up_name As String)
        For Each aUp In upgrades
            If aUp.name = up_name Then
                a_ship.Upgrading = aUp
            End If
        Next
    End Sub
End Class