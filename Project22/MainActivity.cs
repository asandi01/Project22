using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace Project22 {
    [Activity(Label = "Project22", MainLauncher = true)]
    public class MainActivity : Activity {
        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Button btnPersona = FindViewById<Button>(Resource.Id.btnPersona);

            btnPersona.Click+=delegate {
                var activityPersona = new Intent(this, typeof(PersonaActivity));
                StartActivity(activityPersona); 
            };
        }
    }
}

