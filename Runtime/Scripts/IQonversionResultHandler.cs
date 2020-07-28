namespace QonversionUnity
{
	internal interface IQonversionResultHandler
	{
		void onSuccessInit(string uid);

		void onErrorInit(string errorMessage);
	}
}