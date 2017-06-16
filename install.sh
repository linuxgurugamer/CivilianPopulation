#!/bin/sh
rm -rf /Volumes/DATA2/KSP/Games/1.3.0/KSP-Sandbox/ksp-civilian-population-mod-*
rm -rf /Volumes/DATA2/KSP/Games/1.3.0/KSP-Sandbox/GameData/CivilianPopulation
cp /Volumes/DATA/git/CivilianPopulation/mod/target/ksp-civilian-population-mod-*.zip /Volumes/DATA2/KSP/Games/1.3.0/KSP-Sandbox/.
cd /Volumes/DATA2/KSP/Games/1.3.0/KSP-Sandbox
unzip ksp-civilian-population-mod-*.zip
