Bu google api'si igrenc otesi biliyorsun. O yuzden notlari dikkkatli okumak lazim

1- Google.Analytics.Data.V1Beta bu kütüphaneyi nuget'ten yuklerken Include PreRelease işaretlemezsen, yanlış kütüphane ekliyor aq
2- Google Cloud'dan 4 tane falan Google Analytics apisi var. Bunlardan "Google Analytics Data API" ye enable etmen lazim
3- Service Account açarak, key oluştur ve json dosyasını indir. Bunu db'ye kaydetmek lazım blog bazlı falan heralde
4- Google Analyttics'e giderek bu service account email adresini yonetici olarak eklemek lazim


//https://developers.google.com/analytics/devguides/reporting/data/v1/quickstart-client-libraries
//https://learn.microsoft.com/tr-tr/dotnet/api/system.environment.setenvironmentvariable?view=net-7.0
//https://cloud.google.com/docs/authentication/application-default-credentials#GAC
//https://developers.google.com/analytics/devguides/reporting/data/v1/api-schema
//https://stackoverflow.com/questions/72167991/how-to-query-data-via-a-netcore-app-from-google-analytics-v4
//https://www.daimto.com/export-data-from-google-analytics-4-with-c/