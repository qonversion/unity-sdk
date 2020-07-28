package com.qonversion.unitywrapper;

public interface IQonversionResultHandler
{
    void onSuccessInit(String uid);
	
	void onErrorInit(String errorMessage);
}