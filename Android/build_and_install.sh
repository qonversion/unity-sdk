#!/bin/bash
./gradlew assembleRelease
mv unitywrapper/build/outputs/aar/unitywrapper-release.aar ../UnitySdkExample/Assets/Qonversion/Plugins/Android/unitywrapper.aar
