Civilian Population fork by Pamynx
==================================

TODO
----

- Add Netherdyne as an agency.
- Fix landing legs.
- Biosphere
- Parts
    - DockingPorts : civieDockingPort.cfg
    - Hydroponics : Hydroponics_large.cfg - Hydroponics_medium.cfg - Hydroponics_small.cfg
    - Mining : laserdrill.cfg - megadrill.cfg
    - Power : reactor.cfg - smallreactor.cfg
    - Recruitment : university.cfg
    - ResourceStorage : wasteWatertank.cfg - waterTank.cfg

Utility
-------

- HousingSize2
- HousingSize3
- HousingSize4

All ok.

- stbiodomeFarmMk2

Almost ok, top attach point should be removed.

- t1CivBiomassTank

Tank for ressources not added : Water and Substrate.

- surfaceAttachHouseSmall configured, internal ok, transfer ok, go out ok.

TODO : Cannot go back in, no ladder.

```
    [LOG 08:21:59.524] Load(Model): CivilianPopulation/Models/Utility/surfaceAttachHouseSmall
    [ERR 08:21:59.599] Triggers on concave MeshColliders are not supported

    [ERR 08:21:59.600] Triggers on concave MeshColliders are not supported
```

    

Ground Parts
------------

Legs are broken, one do not deploy, the other deploy on load.

@see http://forum.kerbalspaceprogram.com/index.php?/topic/135250-landing-legs-in-11/&page=1
@see KerbalPlanetaryBaseSystems - GameData/PlanetaryBaseInc/BaseSystem/Parts/Wheels/LandingLeg.cfg

Structural Parts
----------------

- truss18x18NoCore
- truss6x18
- truss6x6Core
- truss6x6CoreL
- truss6x6CoreT
- truss6x6CoreX
- truss6x6NoCore
- bioSphereBaseNoWalls

All seems ok.

- bioSphereWindows
- bioSphereWindowsLarge
- bioSphereWindowsWide

Cannot go inside, should improve attache attach points.

- bioDomeBase
- bioDomeBaseLarge
- bioSphereBase
- bioSphereBaseWallRing
- parkbioDomeBase
- parkbioDomeBaseRock
- parkbioDomeBaseMetal

Attachment point in the air, cannot go "into" the structure.

Agency
------

TODO.

@see KerbalPlanetaryBaseSystems - GameData/PlanetaryBaseInc/BaseSystem/Agencies
@see KerbalPlanetaryBaseSystems - Sources/PlanetarySurfaceStructures/SurfaceStructuresCategoryFilter.cs
