package com.qonversion.unitywrapper;

public class QonversionSdkNotInitializedException extends RuntimeException {
    public QonversionSdkNotInitializedException() {
        super();
    }

    public QonversionSdkNotInitializedException(String message) {
        super(message);
    }
}