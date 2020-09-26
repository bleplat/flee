Public Class Upgrade
    Public Name As String = "Debug"
    Public Desc As String = "Ce nom ne peut pas etre plus explicite"
    Public File As String = "Ups" : Public PTN As New Point(0, 0)
    Public Time As ULong = 0
    Public Need As String = ""
    Public Effect As String = ""
    Public Install As Boolean = True
    Public First As Boolean = False

    Public cost As MaterialSet = New MaterialSet()
    Public not_for_bots As Boolean = False


    Public Shared Upgrades As New List(Of Upgrade)
    Public Shared UpsMax As Integer = 10

    Public Shared Sub LoadRegUpgrades()
        Upgrades.Add(New Upgrade With {.Name = "Free", .File = "Ups", .PTN = New Point(4, 7), .cost = New MaterialSet(), .Time = 10, .Need = "", .Effect = "!Suicide", .Install = False, .not_for_bots = True, .Desc = "Release this ship's crew."})
        'Peintures
        Upgrades.Add(New Upgrade With {.Name = "Paint_1", .File = "Ups", .PTN = New Point(5, 1), .cost = New MaterialSet(0, 0, 0, 0), .Time = 25, .Need = "", .Effect = "!C:Lime", .Install = False, .not_for_bots = True, .Desc = "Apply paint."})
        Upgrades.Add(New Upgrade With {.Name = "Paint_2", .File = "Ups", .PTN = New Point(5, 2), .cost = New MaterialSet(0, 0, 0, 0), .Time = 25, .Need = "", .Effect = "!C:Cyan", .Install = False, .not_for_bots = True, .Desc = "Apply paint."})
        Upgrades.Add(New Upgrade With {.Name = "Paint_3", .File = "Ups", .PTN = New Point(5, 3), .cost = New MaterialSet(0, 0, 0, 0), .Time = 25, .Need = "", .Effect = "!C:Yellow", .Install = False, .not_for_bots = True, .Desc = "Apply paint."})
        Upgrades.Add(New Upgrade With {.Name = "Paint_4", .File = "Ups", .PTN = New Point(5, 4), .cost = New MaterialSet(0, 0, 0, 0), .Time = 25, .Need = "", .Effect = "!C:Orange", .Install = False, .not_for_bots = True, .Desc = "Apply paint."})
        Upgrades.Add(New Upgrade With {.Name = "Paint_5", .File = "Ups", .PTN = New Point(5, 5), .cost = New MaterialSet(0, 0, 0, 0), .Time = 25, .Need = "", .Effect = "!C:Blue", .Install = False, .not_for_bots = True, .Desc = "Apply paint."})
        Upgrades.Add(New Upgrade With {.Name = "Paint_6", .File = "Ups", .PTN = New Point(5, 6), .cost = New MaterialSet(0, 0, 0, 0), .Time = 25, .Need = "", .Effect = "!C:White", .Install = False, .not_for_bots = True, .Desc = "Apply paint."})
        Upgrades.Add(New Upgrade With {.Name = "Paint_7", .File = "Ups", .PTN = New Point(5, 7), .cost = New MaterialSet(0, 0, 0, 0), .Time = 25, .Need = "", .Effect = "!C:DimGray", .Install = False, .not_for_bots = True, .Desc = "Apply paint."})

        Upgrades.Add(New Upgrade With {.Name = "Repair_Armor", .File = "Ups", .PTN = New Point(5, 0), .cost = New MaterialSet(200, 0, 0, 0), .Time = 150, .Need = "", .Effect = "!Fix:10", .Install = False, .Desc = "Help fixing the hull."})
        Upgrades.Add(New Upgrade With {.Name = "Tardisication", .File = "Ups", .PTN = New Point(4, 0), .cost = New MaterialSet(1500, 16, 1, 1500), .Time = 50, .Need = "", .Effect = "!UpsMax:1", .Install = False, .not_for_bots = True, .Desc = "Break physics laws and increase the space inside the ship, allowing more upgrades."})
        Upgrades.Add(New Upgrade With {.Name = "Fielbots", .File = "Ups", .PTN = New Point(5, 0), .cost = New MaterialSet(200, 0, 0, 0), .Time = 50, .Need = "?Up:Auto_Nanobots", .Effect = "!FixSFull", .Install = False, .Desc = "Help fixing the shield"})

        Upgrades.Add(New Upgrade With {.Name = "Break_Uranium", .File = "Ups2", .PTN = New Point(7, 7), .cost = New MaterialSet(-1200, -8, 1, 0), .Time = 1, .Need = "?Type:Station", .Effect = "!Fix:1", .Install = False, .Desc = "Sacrifice some Uranium for crystal AndAlso metal."})

        'Special
        Upgrades.Add(New Upgrade With {.Name = "Build_Nuke", .File = "Nuke", .PTN = New Point(0, 0), .cost = New MaterialSet(8000, 32, 8, 2000), .Time = 250, .Need = "?Type:Ambassador", .Effect = "!Sum:Nuke", .Install = False, .Desc = "When something go wrong, it still can go worse." & vbNewLine & "/!\ Playing with the atom may lead to unexpected results."})
        Upgrades.Add(New Upgrade With {.Name = "Launch_Comet", .File = "Comet", .PTN = New Point(0, 0), .cost = New MaterialSet(2000, 16, 0, 0), .Time = 100, .Need = "?Type:Nothing", .Effect = "!Sum:Comet", .Install = False, .Desc = "Launch a comet you can control"})
        Upgrades.Add(New Upgrade With {.Name = "Launch_MSL", .File = "MSL", .PTN = New Point(0, 0), .cost = New MaterialSet(1000, 1, 0, 0), .Time = 100, .Need = "?Type:Scout", .Effect = "!Sum:MSL", .Install = False, .Desc = "Launch a missile you can control"})
        Upgrades.Add(New Upgrade With {.Name = "Launch_MSL_from_yerka", .File = "MSL", .PTN = New Point(0, 0), .cost = New MaterialSet(400, 1, 0, 0), .Time = 100, .Need = "?Type:Yerka", .Effect = "!Sum:MSL", .Install = False, .Desc = "Launch a missile you can control"})
        Upgrades.Add(New Upgrade With {.Name = "Launch_2_MSL", .File = "MSL", .PTN = New Point(0, 0), .cost = New MaterialSet(1000, 0, 0, 0), .Time = 10, .Need = "?Up:Nothing", .Effect = "!Sum:MSL !Sum:MSL", .Install = False, .Desc = "Launch a missile you can control"})
        Upgrades.Add(New Upgrade With {.Name = "Launch_8_MSL", .File = "MSL", .PTN = New Point(0, 0), .cost = New MaterialSet(1000, 0, 0, 0), .Time = 100, .Need = "?Up:Nothing", .Effect = "!Sum:MSL !Sum:MSL !Sum:MSL !Sum:MSL !Sum:MSL !Sum:MSL !Sum:MSL !Sum:MSL", .Install = False, .Desc = "Launch a missile you can control"})
        Upgrades.Add(New Upgrade With {.Name = "Launch_MSL_instant", .File = "MSL", .PTN = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .Time = 0, .Need = "?Up:Nothing", .Effect = "!Sum:MSL", .Install = False, .Desc = "Launch a missile you can control"})
        Upgrades.Add(New Upgrade With {.Name = "Ascend", .File = "Ups", .PTN = New Point(7, 7), .cost = New MaterialSet(0, 0, 32, 0), .Time = 600, .Need = "?Up:Warp_Drive", .Effect = "!Ascend %Speed:300 %Life:100 !Regen:16 %Wloadmax:-100 !Shield:600 !Shieldop:100 !C:White %Shieldreg:2500", .Install = True, .not_for_bots = True, .Desc = "Reach the next level of existence."})
        Upgrades.Add(New Upgrade With {.Name = "Build_Kastou", .File = "Kastou", .PTN = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .Time = 1900, .Need = "?Up:Nothing", .Effect = "!Sum:Kastou", .Install = False, .Desc = "Ya kastou itsi vioctry granti" & vbNewLine & "'yi granti!'"})
        Upgrades.Add(New Upgrade With {.Name = "Build_Crusher", .File = "Crusher", .PTN = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .Time = 3500, .Need = "?Up:Nothing", .Effect = "!Sum:Crusher", .Install = False, .Desc = "It's gonna hurt!" & vbNewLine & "'Hmm... How could you even read this?'"})
        Upgrades.Add(New Upgrade With {.Name = "Build_Ori", .File = "Ori", .PTN = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .Time = 100, .Need = "?Up:Nothing", .Effect = "!Sum:Ori", .Install = False, .Desc = "Hey"})
        Upgrades.Add(New Upgrade With {.Name = "Bugs", .File = "Bugs", .PTN = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .Time = 100, .Need = "?Type:Nothing", .Effect = "!Sum:Bugs", .Install = False, .Desc = "What is this thing?"})
        Upgrades.Add(New Upgrade With {.Name = "Build_Finalizer", .File = "Finalizer", .PTN = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .Time = 600, .Need = "?Up:Ascend", .Effect = "!Sum:Finalizer", .Install = False, .Desc = "Yet, not everybody made it through." & vbNewLine & "We will return for them."})
        Upgrades.Add(New Upgrade With {.Name = "Auto_Nanobots", .File = "Ups", .PTN = New Point(5, 0), .cost = New MaterialSet(0, 0, 0, 0), .Time = 50, .Need = "?Type:Nothing", .Effect = "%Life:1000 !Regen:1000", .Install = True, .Desc = "Gives lots of hull"})

        'Jump
        Upgrades.Add(New Upgrade With {.Name = "Jump", .File = "Ups2", .PTN = New Point(0, 1), .cost = New MaterialSet(0, 1, 0, 0), .Time = 150, .Need = "?Up:Jump_Engine", .Effect = "!Jump:58", .Install = False, .Desc = "Jump."})
        Upgrades.Add(New Upgrade With {.Name = "Jump_II", .File = "Ups2", .PTN = New Point(0, 3), .cost = New MaterialSet(0, 1, 0, 0), .Time = 200, .Need = "?Up:Jump_Engine_2", .Effect = "!Jump:82", .Install = False, .Desc = "Higher Jump, but takes longer to load."})
        Upgrades.Add(New Upgrade With {.Name = "Warp", .File = "Ups2", .PTN = New Point(0, 5), .cost = New MaterialSet(500, 2, 0, 0), .Time = 250, .Need = "?Up:Warp_Drive", .Effect = "!Teleport:1", .Install = False, .Desc = "Warp anywhere. " & vbNewLine & "This jump takes a while to load."})

        Upgrades.Add(New Upgrade With {.Name = "BomberFactory", .File = "BomberFactory", .PTN = New Point(0, 0), .cost = New MaterialSet(3300, 12, 0, 0), .Time = 300, .Need = "?Type:Outpost", .Effect = "!Type:BomberFactory", .Install = True, .Desc = "Change into an automated ship factory"})
        '1->2
        Upgrades.Add(New Upgrade With {.Name = "Improve_To_Scout", .File = "Scout", .PTN = New Point(0, 0), .cost = New MaterialSet(1300, 3, 0, 0), .Time = 300, .Need = "?Type:Simpleship", .Effect = "!Type:Scout", .Install = True, .Desc = "Change into scout ship."})
        Upgrades.Add(New Upgrade With {.Name = "Improve_To_Bomber", .File = "Bomber", .PTN = New Point(0, 0), .cost = New MaterialSet(1600, 3, 0, 0), .Time = 300, .Need = "?Type:Simpleship", .Effect = "!Type:Bomber", .Install = True, .Desc = "Change into attack ship."})
        Upgrades.Add(New Upgrade With {.Name = "Improve_To_Artillery", .File = "Artillery", .PTN = New Point(0, 0), .cost = New MaterialSet(1600, 3, 0, 0), .Time = 300, .Need = "?Type:Simpleship", .Effect = "!Type:Artillery", .Install = True, .Desc = "Change into artillery."})
        Upgrades.Add(New Upgrade With {.Name = "Improve_To_Dronner", .File = "Dronner", .PTN = New Point(0, 0), .cost = New MaterialSet(2000, 5, 0, 0), .Time = 300, .Need = "?Type:Simpleship", .Effect = "!Type:Dronner", .Install = True, .Desc = "Change into ship builder."})

        '2->3
        'Upgrades.Add(New Upgrade With {.Name = "Purger_Dronner", .File = "Purger_Dronner", .PTN = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .CostC = 8, .CostM = 2800, .CostU = 1, .Time = 200, .Need = "?Type:Dronner", .Effect = "!Type:Purger_Dronner", .Install = True, .Desc = "Dronner building different types of drones." & vbNewLine & "/!\ You will not be able to keep building the old ones!"})

        'Deflector
        Upgrades.Add(New Upgrade With {.Name = "Deflector", .File = "Deflected", .PTN = New Point(0, 0), .cost = New MaterialSet(400, 1, 0, 25), .Time = 200, .Need = "", .Effect = "!Deflector:1", .Install = True, .Desc = "As an alternative to shields, this device block any projectile, but is very slow to reload." & vbNewLine & "Due to the way it works, It is not efficient against fast reloading weapons."})
        Upgrades.Add(New Upgrade With {.Name = "Deflector_II", .File = "Deflected", .PTN = New Point(1, 0), .cost = New MaterialSet(500, 2, 0, 25), .Time = 200, .Need = "+Lvl:2" & NPU(), .Effect = "!Deflector:1", .Install = True, .Desc = "Block another projectile."})
        Upgrades.Add(New Upgrade With {.Name = "Deflector_III", .File = "Deflected", .PTN = New Point(2, 0), .cost = New MaterialSet(600, 4, 0, 25), .Time = 200, .Need = "" & NPU(), .Effect = "!Deflector:1", .Install = True, .Desc = "Block another projectile."})
        Upgrades.Add(New Upgrade With {.Name = "Deflector_IV", .File = "Deflected", .PTN = New Point(3, 0), .cost = New MaterialSet(7000, 8, 0, 25), .Time = 200, .Need = "" & NPU(), .Effect = "!Deflector:1", .Install = True, .Desc = "Block another projectile."})
        Upgrades.Add(New Upgrade With {.Name = "Hot_Deflector", .File = "Deflected2", .PTN = New Point(4, 0), .cost = New MaterialSet(900, 3, 0, 100), .Time = 200, .Need = "?Up:Deflector", .Effect = "!HotDeflector:8", .Install = True, .Desc = "This unreliable device was create in an attempt to fix the Deflector vulnerabilities."})
        Upgrades.Add(New Upgrade With {.Name = "Hot_Deflector_II", .File = "Deflected2", .PTN = New Point(5, 0), .cost = New MaterialSet(1100, 3, 0, 100), .Time = 200, .Need = "+Lvl:2" & NPU(), .Effect = "!HotDeflector:8", .Install = True, .Desc = "The Hot Deflector is twice as reliable."})
        Upgrades.Add(New Upgrade With {.Name = "Cold_Deflector", .File = "Deflected3", .PTN = New Point(7, 0), .cost = New MaterialSet(2300, 6, 0, 150), .Time = 200, .Need = "+Lvl:2 ?Up:Deflector", .Effect = "!ColdDeflector:1", .Install = True, .Desc = "This deflector is able to catch an incredible amount of fire, but prevent your weapons from reloading while it is recovering, AndAlso cause damage to your hull from the inside."})

        'Boucliers
        Upgrades.Add(New Upgrade With {.Name = "Shield", .File = "Ups", .PTN = New Point(3, 0), .cost = New MaterialSet(200, 1, 0, 0), .Time = 100, .Need = "", .Effect = "!Shield:25 !Shieldop:25", .Install = True, .Desc = "Protection shield blocking 25% of incomming damages."})
        Upgrades.Add(New Upgrade With {.Name = "Shield_II", .File = "Ups", .PTN = New Point(3, 1), .cost = New MaterialSet(400, 1, 0, 0), .Time = 100, .Need = "" & NPU(), .Effect = "%Shield:20 !Shieldop:25", .Install = True, .Desc = "Shield stop 50% of the shoots AndAlso increase in durability."})
        Upgrades.Add(New Upgrade With {.Name = "Shield_III", .File = "Ups", .PTN = New Point(3, 2), .cost = New MaterialSet(600, 1, 0, 0), .Time = 100, .Need = "" & NPU(), .Effect = "%Shield:20 !Shieldop:25", .Install = True, .Desc = "Shield stop 75% of the shoots AndAlso increase in durability."})
        Upgrades.Add(New Upgrade With {.Name = "Shield_IV", .File = "Ups", .PTN = New Point(3, 3), .cost = New MaterialSet(800, 2, 0, 50), .Time = 100, .Need = "" & NPU(), .Effect = "%Shield:20 !Shieldop:25", .Install = True, .Desc = "Shield stop 100% of the shoots AndAlso increase in durability"})
        Upgrades.Add(New Upgrade With {.Name = "Shield_Capacitor", .File = "Ups", .PTN = New Point(3, 4), .cost = New MaterialSet(600, 1, 0, 0), .Time = 100, .Need = "?Up:Shield", .Effect = "%Shield:50", .Install = True, .Desc = "Shield last 50% longer."})
        Upgrades.Add(New Upgrade With {.Name = "Shield_Capacitor_II", .File = "Ups", .PTN = New Point(3, 5), .cost = New MaterialSet(700, 2, 0, 0), .Time = 100, .Need = "" & NPU(), .Effect = "%Shield:50", .Install = True, .Desc = "Shield last 50% longer."})
        Upgrades.Add(New Upgrade With {.Name = "Shield_Loader", .File = "Ups", .PTN = New Point(3, 7), .cost = New MaterialSet(800, 1, 0, 100), .Time = 100, .Need = "?Up:Shield", .Effect = "%Shieldreg:50", .Install = True, .Desc = "Shield recover 50% faster."})

        'Bouclier (autre)
        Upgrades.Add(New Upgrade With {.Name = "Dynamic_Shield", .File = "Ups2", .PTN = New Point(1, 0), .cost = New MaterialSet(700, 2, 0, 0), .Time = 100, .Need = "+Lvl:2 ?Up:Shield", .Effect = "%Shieldreg:100 %Shield:-50", .Install = True, .Desc = "Shield recover twice as fast, but last half as long."})
        Upgrades.Add(New Upgrade With {.Name = "Partial_Shield", .File = "Ups2", .PTN = New Point(1, 1), .cost = New MaterialSet(700, 2, 0, 0), .Time = 100, .Need = "+Lvl:2 ?Up:Shield_II ?Up:Shield_Loader", .Effect = "%Shieldreg:75 !Shieldop:-25", .Install = True, .Desc = "Shield recover 75% faster, but let 25% of shoot damages pass."})
        Upgrades.Add(New Upgrade With {.Name = "Surface_Shield", .File = "Ups2", .PTN = New Point(1, 2), .cost = New MaterialSet(700, 2, 0, 0), .Time = 100, .Need = "+Lvl:2 ?Up:Shield_II", .Effect = "%Shield:75 !Shieldop:-25", .Install = True, .Desc = "Shield last 75% longer, but let 25% of shoot damages pass."})
        Upgrades.Add(New Upgrade With {.Name = "Alternative_Shield", .File = "Ups2", .PTN = New Point(1, 3), .cost = New MaterialSet(700, 3, 0, 100), .Time = 100, .Need = "+Lvl:2 ?Up:Shield_Loader", .Effect = "%Shieldreg:25 %Shield:25 !Shieldop:-50", .Install = True, .Desc = "Shield recover 25% faster AndAlso last 25% longer, but let 25% of shoot damages pass."})
        Upgrades.Add(New Upgrade With {.Name = "Bidirectional_Shield", .File = "Ups2", .PTN = New Point(1, 4), .cost = New MaterialSet(700, 2, 0, 100), .Time = 100, .Need = "+Lvl:1 ?Up:Shield", .Effect = "%Shield:25 %Wloadmax:15", .Install = True, .Desc = "Shield last 25% longer, but weapons shoot slower."})
        Upgrades.Add(New Upgrade With {.Name = "Shield_Capacitor_III", .File = "Ups2", .PTN = New Point(3, 2), .cost = New MaterialSet(1200, 3, 1, 0), .Time = 200, .Need = "?Up:Shield_Capacitor_II", .Effect = "%Shield:75", .Install = True, .Desc = "Shield last 50% longer."})
        Upgrades.Add(New Upgrade With {.Name = "Shield_Loader_II", .File = "Ups2", .PTN = New Point(3, 4), .cost = New MaterialSet(1200, 3, 0, 200), .Time = 200, .Need = "?Up:Shield_Loader", .Effect = "%Shieldreg:35", .Install = True, .Desc = "Shield recover 35% faster."})
        Upgrades.Add(New Upgrade With {.Name = "Shield_Patch", .File = "Ups2", .PTN = New Point(3, 0), .cost = New MaterialSet(600, 1, 1, 200), .Time = 200, .Need = "?Up:Shield_IV", .Effect = "!Shieldop:25", .Install = True, .Desc = "Shield stop +25% of the shoots (this can fix some of the other upgrades effects)"})

        'Rechagement
        Upgrades.Add(New Upgrade With {.Name = "Reloader", .File = "Ups", .PTN = New Point(2, 0), .cost = New MaterialSet(100, 1, 0, 0), .Time = 100, .Need = "+Lvl:1 ?W", .Effect = "%Wloadmax:-15", .Install = True, .Desc = "Weapons reload 15% faster."})
        Upgrades.Add(New Upgrade With {.Name = "Reloader_II", .File = "Ups", .PTN = New Point(2, 1), .cost = New MaterialSet(200, 1, 0, 0), .Time = 100, .Need = "?W" & NPU(), .Effect = "%Wloadmax:-15", .Install = True, .Desc = "Weapons reload 15% faster."})
        Upgrades.Add(New Upgrade With {.Name = "Reloader_III", .File = "Ups", .PTN = New Point(2, 2), .cost = New MaterialSet(300, 1, 0, 0), .Time = 100, .Need = "?W" & NPU(), .Effect = "%Wloadmax:-15", .Install = True, .Desc = "Weapons reload 15% faster."})
        Upgrades.Add(New Upgrade With {.Name = "Reloader_IV", .File = "Ups", .PTN = New Point(2, 3), .cost = New MaterialSet(400, 1, 0, 0), .Time = 100, .Need = "?W" & NPU(), .Effect = "%Wloadmax:-15", .Install = True, .Desc = "Weapons reload 15% faster."})
        Upgrades.Add(New Upgrade With {.Name = "Reloader_V", .File = "Ups", .PTN = New Point(2, 4), .cost = New MaterialSet(500, 1, 0, 0), .Time = 100, .Need = "?W" & NPU(), .Effect = "%Wloadmax:-15", .Install = True, .Desc = "Weapons reload 15% faster."})
        Upgrades.Add(New Upgrade With {.Name = "Reloader_VI", .File = "Ups", .PTN = New Point(2, 5), .cost = New MaterialSet(600, 1, 0, 0), .Time = 100, .Need = "?W" & NPU(), .Effect = "%Wloadmax:-15", .Install = True, .Desc = "Weapons reload 15% faster."})
        Upgrades.Add(New Upgrade With {.Name = "Reloader_VII", .File = "Ups", .PTN = New Point(2, 6), .cost = New MaterialSet(700, 1, 0, 0), .Time = 100, .Need = "?W" & NPU(), .Effect = "%Wloadmax:-15", .Install = True, .Desc = "Weapons reload 15% faster."})
        Upgrades.Add(New Upgrade With {.Name = "Reloader_VIII", .File = "Ups", .PTN = New Point(2, 7), .cost = New MaterialSet(800, 1, 0, 0), .Time = 100, .Need = "?W" & NPU(), .Effect = "%Wloadmax:-15", .Install = True, .Desc = "Weapons reload 15% faster."})

        'Barilet+
        Upgrades.Add(New Upgrade With {.Name = "Double_Barrel", .File = "Ups", .PTN = New Point(7, 0), .cost = New MaterialSet(900, 1, 0, 0), .Time = 100, .Need = "?W", .Effect = "%Wbar:100 %Wloadmax:80", .Install = True, .Desc = "Loading is 80% longer, but weapons get double shoot." & vbNewLine & "'Two barrilets arround an axis.'"})
        Upgrades.Add(New Upgrade With {.Name = "Double_Double_Barrel", .File = "Ups", .PTN = New Point(7, 5), .cost = New MaterialSet(1200, 1, 0, 0), .Time = 120, .Need = "+Lvl:2 ?W" & NPU(), .Effect = "%Wbar:100 %Wloadmax:90", .Install = True, .Desc = "Loading is 90% longer, but weapons get double shoot again." & vbNewLine & "'Two groups, of two barrilets arround an axis, arround an axis.'"})
        Upgrades.Add(New Upgrade With {.Name = "Double_Double_Barrel_Doubled", .File = "Ups", .PTN = New Point(7, 6), .cost = New MaterialSet(1400, 1, 0, 0), .Time = 140, .Need = "+Lvl:2 ?W" & NPU(), .Effect = "%Wbar:100 %Wloadmax:100", .Install = True, .Desc = "Loading is twice as much expensive in time, but they get an awesome improvement." & vbNewLine & "'Two groups, of two barrilets arround an axis, arround an axis. AndAlso itself doubled arround another axis. Trust the engineers.'"})

        'Propultion
        Upgrades.Add(New Upgrade With {.Name = "Steering", .File = "Ups2", .PTN = New Point(2, 0), .cost = New MaterialSet(200, 0, 0, 0), .Time = 100, .Need = "?S", .Effect = "!Agility:1", .Install = True, .Desc = "Ship turn faster."})
        Upgrades.Add(New Upgrade With {.Name = "Steering_II", .File = "Ups2", .PTN = New Point(2, 1), .cost = New MaterialSet(400, 0, 0, 25), .Time = 150, .Need = "?S" & NPU(), .Effect = "!Agility:1", .Install = True, .Desc = "Ship turn faster."})
        Upgrades.Add(New Upgrade With {.Name = "Steering_III", .File = "Ups2", .PTN = New Point(2, 2), .cost = New MaterialSet(1200, 2, 0, 50), .Time = 200, .Need = "?S" & NPU(), .Effect = "!Agility:1", .Install = True, .Desc = "Ship turn faster."})
        Upgrades.Add(New Upgrade With {.Name = "Engines", .File = "Ups", .PTN = New Point(1, 0), .cost = New MaterialSet(200, 1, 0, 0), .Time = 100, .Need = "?S", .Effect = "%Speed:5", .Install = True, .Desc = "Ship is 5% faster."})
        Upgrades.Add(New Upgrade With {.Name = "Engines_II", .File = "Ups", .PTN = New Point(1, 1), .cost = New MaterialSet(200, 1, 0, 0), .Time = 105, .Need = "?S" & NPU(), .Effect = "%Speed:10", .Install = True, .Desc = "Ship is 10% faster."})
        Upgrades.Add(New Upgrade With {.Name = "Engines_III", .File = "Ups", .PTN = New Point(1, 2), .cost = New MaterialSet(200, 1, 0, 0), .Time = 110, .Need = "?S" & NPU(), .Effect = "%Speed:10", .Install = True, .Desc = "Ship is 10% faster."})
        Upgrades.Add(New Upgrade With {.Name = "Engines_IV", .File = "Ups", .PTN = New Point(1, 3), .cost = New MaterialSet(200, 1, 0, 0), .Time = 115, .Need = "?S" & NPU(), .Effect = "%Speed:5", .Install = True, .Desc = "Ship is 5% faster."})
        Upgrades.Add(New Upgrade With {.Name = "Engines_V", .File = "Ups", .PTN = New Point(1, 4), .cost = New MaterialSet(200, 1, 0, 25), .Time = 120, .Need = "?S" & NPU(), .Effect = "%Speed:5", .Install = True, .Desc = "Ship is 5% faster."})
        Upgrades.Add(New Upgrade With {.Name = "Engines_VI", .File = "Ups", .PTN = New Point(1, 5), .cost = New MaterialSet(200, 1, 0, 50), .Time = 125, .Need = "?S" & NPU(), .Effect = "%Speed:10", .Install = True, .Desc = "Ship is 10% faster."})
        Upgrades.Add(New Upgrade With {.Name = "Engines_VII", .File = "Ups", .PTN = New Point(1, 6), .cost = New MaterialSet(200, 1, 0, 75), .Time = 130, .Need = "?S" & NPU(), .Effect = "%Speed:10", .Install = True, .Desc = "Ship is 10% faster."})
        Upgrades.Add(New Upgrade With {.Name = "Engines_VIII", .File = "Ups", .PTN = New Point(1, 7), .cost = New MaterialSet(200, 1, 0, 100), .Time = 140, .Need = "?S" & NPU(), .Effect = "%Speed:10", .Install = True, .Desc = "Ship is 10% faster."})

        'Sauts
        Upgrades.Add(New Upgrade With {.Name = "Jump_Engine", .File = "Ups2", .PTN = New Point(0, 0), .cost = New MaterialSet(800, 2, 0, 0), .Time = 200, .Need = "?S", .Effect = "", .Install = True, .Desc = "Allow the ship to make jumps, at the price of crystals."})
        Upgrades.Add(New Upgrade With {.Name = "Jump_Engine_2", .File = "Ups2", .PTN = New Point(0, 2), .cost = New MaterialSet(1000, 2, 0, 25), .Time = 250, .Need = "?S ?Up:Jump_Engine", .Effect = "", .Install = True, .Desc = "Allow higher jumps, but is more expensive in crystals."})
        Upgrades.Add(New Upgrade With {.Name = "Warp_Drive", .File = "Ups2", .PTN = New Point(0, 4), .cost = New MaterialSet(1200, 3, 0, 600), .Time = 300, .Need = "", .Effect = "", .Install = True, .Desc = "Allow you to warp anywhere."})

        'Blindage
        Upgrades.Add(New Upgrade With {.Name = "Hull", .File = "Ups", .PTN = New Point(0, 0), .cost = New MaterialSet(200, 0, 0, 0), .Time = 80, .Need = "", .Effect = "%Life:5", .Install = True, .Desc = "Increase the hull."})
        Upgrades.Add(New Upgrade With {.Name = "Hull_II", .File = "Ups", .PTN = New Point(0, 1), .cost = New MaterialSet(300, 0, 0, 0), .Time = 90, .Need = "" & NPU(), .Effect = "%Life:10", .Install = True, .Desc = "Increase the hull."})
        Upgrades.Add(New Upgrade With {.Name = "Hull_III", .File = "Ups", .PTN = New Point(0, 2), .cost = New MaterialSet(600, 0, 0, 0), .Time = 100, .Need = "" & NPU(), .Effect = "%Life:10", .Install = True, .Desc = "Increase the hull."})
        Upgrades.Add(New Upgrade With {.Name = "Hull_IV", .File = "Ups", .PTN = New Point(0, 3), .cost = New MaterialSet(1300, 0, 0, 0), .Time = 110, .Need = "" & NPU(), .Effect = "%Life:10", .Install = True, .Desc = "Increase the hull."})
        Upgrades.Add(New Upgrade With {.Name = "Hull_V", .File = "Ups", .PTN = New Point(0, 4), .cost = New MaterialSet(1900, 0, 0, 0), .Time = 120, .Need = "" & NPU(), .Effect = "%Life:10", .Install = True, .Desc = "Increase the hull."})
        Upgrades.Add(New Upgrade With {.Name = "Hull_VI", .File = "Ups", .PTN = New Point(0, 5), .cost = New MaterialSet(2500, 0, 0, 0), .Time = 130, .Need = "" & NPU(), .Effect = "%Life:10", .Install = True, .Desc = "Increase the hull."})
        Upgrades.Add(New Upgrade With {.Name = "Heavy_Hull", .File = "Ups", .PTN = New Point(0, 6), .cost = New MaterialSet(3000, 0, 0, 200), .Time = 140, .Need = "" & NPU(), .Effect = "%Life:15", .Install = True, .Desc = "Increase the hull."})
        Upgrades.Add(New Upgrade With {.Name = "Heavy_Hull_II", .File = "Ups", .PTN = New Point(0, 7), .cost = New MaterialSet(4000, 0, 0, 400), .Time = 150, .Need = "" & NPU(), .Effect = "%Life:20", .Install = True, .Desc = "Increase the hull, but half the ship speed."})

        'Regen+
        Upgrades.Add(New Upgrade With {.Name = "Repair_Bots", .File = "Ups", .PTN = New Point(7, 1), .cost = New MaterialSet(400, 1, 0, 0), .Time = 100, .Need = "+Life:100", .Effect = "!Regen:1", .Install = True, .Desc = "Repair the ship faster."})
        Upgrades.Add(New Upgrade With {.Name = "Repair_Bots_II", .File = "Ups", .PTN = New Point(7, 2), .cost = New MaterialSet(700, 1, 0, 0), .Time = 120, .Need = "+Life:200" & NPU(), .Effect = "!Regen:1", .Install = True, .Desc = "Repair the ship faster."})
        Upgrades.Add(New Upgrade With {.Name = "Repair_Bots_III", .File = "Ups", .PTN = New Point(7, 3), .cost = New MaterialSet(1400, 1, 0, 0), .Time = 130, .Need = "+Life:300" & NPU(), .Effect = "!Regen:1", .Install = True, .Desc = "Repair the ship faster."})
        Upgrades.Add(New Upgrade With {.Name = "Repair_Bots_IV", .File = "Ups", .PTN = New Point(7, 4), .cost = New MaterialSet(2000, 2, 0, 0), .Time = 150, .Need = "+Life:400" & NPU(), .Effect = "!Regen:1", .Install = True, .Desc = "Repair the ship faster."})

        'BASE radio
        Upgrades.Add(New Upgrade With {.Name = "Multiunit_Control", .File = "Ups", .PTN = New Point(6, 0), .cost = New MaterialSet(1000, 1, 0, 0), .Time = 100, .Need = "?Base", .Effect = "!Maxships:2", .Install = True, .Desc = "Control 2 more ships"})
        Upgrades.Add(New Upgrade With {.Name = "Multiunit_Control_II", .File = "Ups", .PTN = New Point(6, 1), .cost = New MaterialSet(2000, 2, 0, 0), .Time = 150, .Need = "?Base" & NPU(), .Effect = "!Maxships:3", .Install = True, .Desc = "Control 3 more ships"})
        Upgrades.Add(New Upgrade With {.Name = "Multiunit_Control_III", .File = "Ups", .PTN = New Point(6, 2), .cost = New MaterialSet(3000, 3, 0, 0), .Time = 200, .Need = "?Base" & NPU(), .Effect = "!Maxships:2", .Install = True, .Desc = "Control 2 more ships"})
        Upgrades.Add(New Upgrade With {.Name = "Multiunit_Control_IV", .File = "Ups", .PTN = New Point(6, 3), .cost = New MaterialSet(4000, 4, 0, 0), .Time = 200, .Need = "?Base" & NPU(), .Effect = "!Maxships:3", .Install = True, .Desc = "Control 3 more ships"})
        Upgrades.Add(New Upgrade With {.Name = "Multiunit_Control_V", .File = "Ups", .PTN = New Point(6, 4), .cost = New MaterialSet(5000, 5, 0, 0), .Time = 250, .Need = "?Base" & NPU(), .Effect = "!Maxships:2", .Install = True, .Desc = "Control 2 more ships"})
        Upgrades.Add(New Upgrade With {.Name = "Multiunit_Control_VI", .File = "Ups", .PTN = New Point(6, 5), .cost = New MaterialSet(6000, 6, 0, 0), .Time = 250, .Need = "?Base" & NPU(), .Effect = "!Maxships:3", .Install = True, .Desc = "Control 3 more ships"})
        Upgrades.Add(New Upgrade With {.Name = "Multiunit_Control_VII", .File = "Ups", .PTN = New Point(6, 6), .cost = New MaterialSet(7000, 7, 0, 100), .Time = 250, .Need = "?Base" & NPU(), .Effect = "!Maxships:2", .Install = True, .Desc = "Control 2 more ships"})
        Upgrades.Add(New Upgrade With {.Name = "Multiunit_Control_VIII", .File = "Ups", .PTN = New Point(6, 7), .cost = New MaterialSet(8000, 8, 0, 200), .Time = 300, .Need = "?Base" & NPU(), .Effect = "!Maxships:2", .Install = True, .Desc = "Control 2 more ships"})

        'BASE ups+
        Upgrades.Add(New Upgrade With {.Name = "Space_Shemes", .File = "Ups", .PTN = New Point(4, 0), .cost = New MaterialSet(1000, 1, 0, 0), .Time = 100, .Need = "?Base", .Effect = "!Upsbonus:2", .Install = True, .Desc = "New ships can install 2 additional upgrades."})
        Upgrades.Add(New Upgrade With {.Name = "Space_Shemes_II", .File = "Ups", .PTN = New Point(4, 1), .cost = New MaterialSet(2000, 2, 0, 0), .Time = 100, .Need = "?Base" & NPU(), .Effect = "!Upsbonus:2", .Install = True, .Desc = "New ships can install 2 additional upgrades."})
        Upgrades.Add(New Upgrade With {.Name = "Space_Shemes_III", .File = "Ups", .PTN = New Point(4, 2), .cost = New MaterialSet(3000, 4, 0, 0), .Time = 100, .Need = "?Base" & NPU(), .Effect = "!Upsbonus:2", .Install = True, .Desc = "New ships can install 2 additional upgrades."})
        Upgrades.Add(New Upgrade With {.Name = "Space_Shemes_IV", .File = "Ups", .PTN = New Point(4, 4), .cost = New MaterialSet(4000, 8, 0, 0), .Time = 100, .Need = "?Base" & NPU(), .Effect = "!Upsbonus:2", .Install = True, .Desc = "New ships can install 2 additional upgrades."})
        Upgrades.Add(New Upgrade With {.Name = "Space_Shemes_V", .File = "Ups", .PTN = New Point(4, 3), .cost = New MaterialSet(5000, 16, 0, 0), .Time = 100, .Need = "?Base" & NPU(), .Effect = "!Upsbonus:2", .Install = True, .Desc = "New ships can install 2 additional upgrades."})

        'BASE Spawn
        Upgrades.Add(New Upgrade With {.Name = "Build_Cargo", .File = "Cargo", .PTN = New Point(0, 0), .cost = New MaterialSet(1200, 0, 0, 0), .Time = 300, .Need = "?MS ?Type:Station", .Effect = "!Sum:Cargo", .Install = False, .Desc = "Build a weak but fast mining ship." & vbNewLine & "'An overpowered ship.'"})
        Upgrades.Add(New Upgrade With {.Name = "Build_Hunter", .File = "Hunter", .PTN = New Point(0, 0), .cost = New MaterialSet(800, 1, 0, 0), .Time = 220, .Need = "?MS ?Type:Station", .Effect = "!Sum:Hunter", .Install = False, .Desc = "Build a light hunter ship."})
        Upgrades.Add(New Upgrade With {.Name = "Build_Harass", .File = "Harass", .PTN = New Point(0, 0), .cost = New MaterialSet(950, 2, 0, 0), .Time = 350, .Need = "?MS ?Type:Station", .Effect = "!Sum:Harass", .Install = False, .Desc = "Build a medium hunter ship."})
        Upgrades.Add(New Upgrade With {.Name = "Build_Simpleship", .File = "Simpleship", .PTN = New Point(0, 0), .cost = New MaterialSet(1000, 1, 0, 0), .Time = 380, .Need = "?MS ?Type:Station", .Effect = "!Sum:Simpleship", .Install = False, .Desc = "Build a basic ship that you can upgrade."})
        Upgrades.Add(New Upgrade With {.Name = "Build_Legend_I", .File = "Legend_I", .PTN = New Point(0, 0), .cost = New MaterialSet(1400, 16, 1, 0), .Time = 650, .Need = "?MS ?Type:Station", .Effect = "!Sum:Legend_I", .Install = True, .Desc = "Build a legendary ship." & vbNewLine & "/!\ You can only build this ship once!"})
        Upgrades.Add(New Upgrade With {.Name = "Build_Legend_K", .File = "Legend_K", .PTN = New Point(0, 0), .cost = New MaterialSet(1200, 14, 0, 400), .Time = 650, .Need = "?MS ?Type:Station", .Effect = "!Sum:Legend_K", .Install = True, .Desc = "Build a legendary ship." & vbNewLine & "/!\ You can only build this ship once!"})
        Upgrades.Add(New Upgrade With {.Name = "Build_Legend_L", .File = "Legend_L", .PTN = New Point(0, 0), .cost = New MaterialSet(1300, 15, 0, 400), .Time = 650, .Need = "?MS ?Type:Station", .Effect = "!Sum:Legend_L", .Install = True, .Desc = "Build a legendary ship." & vbNewLine & "/!\ You can only build this ship once!"})
        Upgrades.Add(New Upgrade With {.Name = "Build_Legend_U", .File = "Legend_U", .PTN = New Point(0, 0), .cost = New MaterialSet(1200, 16, 0, 200), .Time = 650, .Need = "?MS ?Type:Station", .Effect = "!Sum:Legend_U", .Install = True, .Desc = "Build a legendary ship." & vbNewLine & "/!\ You can only build this ship once!"})
        Upgrades.Add(New Upgrade With {.Name = "Build_Legend_Y", .File = "Legend_Y", .PTN = New Point(0, 0), .cost = New MaterialSet(1400, 14, 0, 300), .Time = 650, .Need = "?MS ?Type:Station", .Effect = "!Sum:Legend_Y", .Install = True, .Desc = "Build a legendary ship." & vbNewLine & "/!\ You can only build this ship once!"})

        'NPC Spawn
        Upgrades.Add(New Upgrade With {.Name = "Build_Dronner", .File = "Dronner", .PTN = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .Time = 750, .Need = "?Up:Nothing", .Effect = "!Sum:Dronner", .Install = False, .Desc = "Construire un constructeur de drones"})
        Upgrades.Add(New Upgrade With {.Name = "Build_Scout", .File = "Scout", .PTN = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .Time = 550, .Need = "?Up:Nothing", .Effect = "!Sum:Scout", .Install = False, .Desc = "Construire un vaisseau d'exploration"})
        Upgrades.Add(New Upgrade With {.Name = "Build_Bomber", .File = "Bomber", .PTN = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .Time = 550, .Need = "?Up:Nothing", .Effect = "!Sum:Bomber", .Install = False, .Desc = "Construire un vaisseau lourd"})
        Upgrades.Add(New Upgrade With {.Name = "Build_Yerka", .File = "Yerka", .PTN = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .Time = 720, .Need = "?Up:Nothing", .Effect = "!Sum:Yerka", .Install = False, .Desc = "Ya Yerka Yi!"})
        Upgrades.Add(New Upgrade With {.Name = "Build_Strange", .File = "Strange", .PTN = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .Time = 850, .Need = "?Up:Nothing", .Effect = "!Sum:Strange", .Install = False, .Desc = ""})
        Upgrades.Add(New Upgrade With {.Name = "Build_Sacred", .File = "Sacred", .PTN = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .Time = 200, .Need = "?Up:Nothing", .Effect = "!Sum:Sacred", .Install = False, .Desc = ""})
        Upgrades.Add(New Upgrade With {.Name = "Build_Artillery", .File = "Artillery", .PTN = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .Time = 270, .Need = "?Up:Nothing", .Effect = "!Sum:Artillery", .Install = False, .Desc = ""})
        Upgrades.Add(New Upgrade With {.Name = "Build_MiniColonizer", .File = "MiniColonizer", .PTN = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .Time = 500, .Need = "?Up:Nothing", .Effect = "!Sum:MiniColonizer", .Install = False, .Desc = ""})
        Upgrades.Add(New Upgrade With {.Name = "Build_Colonizer", .File = "Colonizer", .PTN = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .Time = 500, .Need = "?Up:Nothing", .Effect = "!Sum:Colonizer", .Install = False, .Desc = ""})
        Upgrades.Add(New Upgrade With {.Name = "Build_Converter_A", .File = "Converter_A", .PTN = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .Time = 220, .Need = "?Type:Converter", .Effect = "!Sum:Converter_A", .Install = False, .Desc = ""})
        Upgrades.Add(New Upgrade With {.Name = "Build_Converter_B", .File = "Converter_B", .PTN = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .Time = 220, .Need = "?Type:Converter", .Effect = "!Sum:Converter_B", .Install = False, .Desc = ""})

        'Drone Spawn
        Upgrades.Add(New Upgrade With {.Name = "Build_Explorer", .File = "Explorer", .PTN = New Point(0, 0), .cost = New MaterialSet(100, 0, 0, 0), .Time = 150, .Need = "?MS ?Type:Dronner", .Effect = "!Sum:Explorer", .Install = False, .Desc = "Build a weak minner drone."})
        Upgrades.Add(New Upgrade With {.Name = "Build_Combat_Drone_1", .File = "Combat_Drone_1", .PTN = New Point(0, 0), .cost = New MaterialSet(370, 0, 0, 0), .Time = 100, .Need = "?MS ?Type:Dronner", .Effect = "!Sum:Combat_Drone_1", .Install = False, .Desc = "Build a combat drone."})
        Upgrades.Add(New Upgrade With {.Name = "Build_Combat_Drone_2", .File = "Combat_Drone_2", .PTN = New Point(0, 0), .cost = New MaterialSet(320, 0, 0, 0), .Time = 100, .Need = "?MS ?Type:Dronner", .Effect = "!Sum:Combat_Drone_2", .Install = False, .Desc = "Build a combat drone."})
        Upgrades.Add(New Upgrade With {.Name = "Build_Combat_Drone_3", .File = "Combat_Drone_3", .PTN = New Point(0, 0), .cost = New MaterialSet(380, 0, 0, 0), .Time = 100, .Need = "?MS ?Type:Dronner", .Effect = "!Sum:Combat_Drone_3", .Install = False, .Desc = "Build a combat drone."})
        Upgrades.Add(New Upgrade With {.Name = "Build_Drone", .File = "Drone", .PTN = New Point(0, 0), .cost = New MaterialSet(800, 1, 0, 0), .Time = 150, .Need = "?MS ?Type:Dronner", .Effect = "!Sum:Drone", .Install = False, .Desc = "Summon a big drone."})

        'Purger Drones
        Upgrades.Add(New Upgrade With {.Name = "Purger_Control", .File = "Ups", .PTN = New Point(6, 7), .cost = New MaterialSet(32000, 32, 0, 0), .Time = 300, .Need = "?Type:Purger_Dronner", .Effect = "!Maxships:8", .Install = True, .Desc = "Control 8 more ships"})
        Upgrades.Add(New Upgrade With {.Name = "Build_Purger_Dronner", .File = "Purger_Dronner", .PTN = New Point(0, 0), .cost = New MaterialSet(0, 0, 0, 0), .Time = 2400, .Need = "?Up:Nothing", .Effect = "!Sum:Purger_Dronner", .Install = False, .Desc = ""})
        Upgrades.Add(New Upgrade With {.Name = "Build_Purger_Drone_1", .File = "Purger_Drone_1", .PTN = New Point(0, 0), .cost = New MaterialSet(400, 0, 0, 0), .Time = 50, .Need = "?MS ?Type:Purger_Dronner", .Effect = "!Sum:Purger_Drone_1", .Install = False, .Desc = "Build a combat drone."})
        Upgrades.Add(New Upgrade With {.Name = "Build_Purger_Drone_2", .File = "Purger_Drone_2", .PTN = New Point(0, 0), .cost = New MaterialSet(450, 0, 0, 0), .Time = 50, .Need = "?MS ?Type:Purger_Dronner", .Effect = "!Sum:Purger_Drone_2", .Install = False, .Desc = "Build a combat drone."})
        Upgrades.Add(New Upgrade With {.Name = "Build_Purger_Drone_3", .File = "Purger_Drone_3", .PTN = New Point(0, 0), .cost = New MaterialSet(475, 0, 0, 0), .Time = 50, .Need = "?MS ?Type:Purger_Dronner", .Effect = "!Sum:Purger_Drone_3", .Install = False, .Desc = "Build a combat drone."})

        'Bases
        Upgrades.Add(New Upgrade With {.Name = "Build_Outpost", .File = "Outpost", .PTN = New Point(0, 0), .cost = New MaterialSet(1050, 3, 0, 0), .Time = 260, .Need = "?MS ?Type:Colonizer", .Effect = "!Sum:Outpost", .Install = False, .Desc = "Build a slow mining outpost."})
        Upgrades.Add(New Upgrade With {.Name = "Build_Defense", .File = "Defense", .PTN = New Point(0, 0), .cost = New MaterialSet(1350, 3, 0, 0), .Time = 220, .Need = "?MS ?Type:Colonizer", .Effect = "!Sum:Defense", .Install = False, .Desc = "Build an high range defense turret."})
        Upgrades.Add(New Upgrade With {.Name = "Build_Pointvortex", .File = "Pointvortex", .PTN = New Point(0, 0), .cost = New MaterialSet(1550, 3, 0, 50), .Time = 220, .Need = "?MS ?Type:Colonizer", .Effect = "!Sum:Pointvortex", .Install = False, .Desc = "Slow AndAlso highest range turret."})

        Upgrades.Add(New Upgrade With {.Name = "Build_PartialBomber", .File = "PartialBomber", .PTN = New Point(0, 0), .cost = New MaterialSet(600, 0, 0, 25), .Time = 2700, .Need = "?MS ?Type:BomberFactory", .Effect = "!Sum:PartialBomber", .Install = False, .Desc = ""})
    End Sub
    Public Shared Function NPU() As String
        Return " ?Up:" & Upgrades(Upgrades.Count - 1).Name
    End Function

    Public Shared Function UpgradeFromName(up_name As String) As Upgrade
        For Each aUp In Upgrades
            If aUp.Name = up_name Then
                Return aUp
            End If
        Next
        Return Nothing
    End Function
    Public Shared Sub ForceUpgradeToShip(a_ship As Ship, up_name As String)
        For Each aUp In Upgrades
            If aUp.Name = up_name Then
                a_ship.Upgrading = aUp
            End If
        Next
    End Sub
End Class