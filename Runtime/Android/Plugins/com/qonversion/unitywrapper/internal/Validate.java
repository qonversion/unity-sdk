package com.qonversion.unitywrapper.internal;

import com.qonversion.unitywrapper.QonversionSdkNotInitializedException;
import com.qonversion.unitywrapper.QonversionWrapper;

public final class Validate {

    private static final String TAG = Validate.class.getName();

    public static void sdkInitialized() {
        if (!QonversionWrapper.isInitialized()) {
            throw new QonversionSdkNotInitializedException(
                    "The SDK has not been initialized, make sure to call "
                            + "QonversionWrapper.Launch() first.");
        }
    }
}