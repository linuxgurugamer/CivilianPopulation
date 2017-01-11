Civilian Population fork by Pamynx
==================================

TODO
----

- Add Netherdyne as an agency.
- Fix landing legs.
- Biosphere
- Parts
    - Biospheres : stbiodomeFarmMk2 - bioSphereWindows - bioSphereWindowsLarge - bioSphereWindowsWide - t1CivBiomassTank - bioDomeBase - bioDomeBaseLarge - bioSphereBase - bioSphereBaseNoWalls - bioSphereBaseWallRing - parkbioDomeBase - parkbioDomeBaseRock - parkbioDomeBaseMetal
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

All seems ok.

Agency
------

TODO.

@see KerbalPlanetaryBaseSystems - GameData/PlanetaryBaseInc/BaseSystem/Agencies
@see KerbalPlanetaryBaseSystems - Sources/PlanetarySurfaceStructures/SurfaceStructuresCategoryFilter.cs
