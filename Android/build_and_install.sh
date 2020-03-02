#!/bin/bash
./gradlew assembleRelease
mv unitywrapper/build/outputs/aar/unitywrapper-release.aar ../UnityExample/Assets/Qonversion/Plugins/Android/unitywrapper.aar
