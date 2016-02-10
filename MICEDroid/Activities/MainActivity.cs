using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace MICEDroid.Activities
{
    [Activity(Label = "MICEDroid", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MICETheme")]
    public class MainActivity : Activity
    {
        Button connect;
        string Username;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.Toolbar);
            EditText username = FindViewById<EditText>(Resource.Id.Username);
            connect = FindViewById<Button>(Resource.Id.Connect);

            connect.Click += Connect_Click;
            username.AfterTextChanged += Username_AfterTextChanged;

            SetActionBar(toolbar);
            ActionBar.Title = "MICE";
        }

        private void Connect_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                Toast.MakeText(this, "Must have username", ToastLength.Long).Show();
            }
            else
            {
                var activity = new Intent(this, typeof(CallActivity));
                activity.PutExtra("Username", Username);
                StartActivity(activity);
            }
        }

        private void Username_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            Username = e.Editable.ToString();
            connect.Text = $"Connect as: {Username}";
        }

        
    }
}

