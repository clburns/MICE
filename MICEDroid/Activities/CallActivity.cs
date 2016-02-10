using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using FM.IceLink;
using FM.IceLink.WebRTC;
using MICEDroid.Codecs.Opus;
using MICEDroid.Codecs.VP8;
using MICEDroid.IceLink;
using MICEPcl;

namespace MICEDroid.Activities
{
    [Activity(Label = "CallActivity", Theme = "@style/MICETheme")]
    public class CallActivity : Activity
    {
        LocalMedia LocalMedia;
        string Username;
        private RelativeLayout Layout;
        private RelativeLayout Container;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CallLayout);
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.Toolbar);
            Layout = FindViewById<RelativeLayout>(Resource.Id.Layout);
            Container = FindViewById<RelativeLayout>(Resource.Id.Container);

            Window.AddFlags(WindowManagerFlags.KeepScreenOn);

            Username = Intent.GetStringExtra("Username") ?? "Data not available";

            SetActionBar(toolbar);
            ActionBar.Title = $"Connected as: {Username}";

            SetupCodecs();

            SetupCall();
        }

        private void SetupCodecs()
        {
            Java.Lang.JavaSystem.LoadLibrary("opus");
            Java.Lang.JavaSystem.LoadLibrary("opusJNI");
            Java.Lang.JavaSystem.LoadLibrary("vpx");
            Java.Lang.JavaSystem.LoadLibrary("vpxJNI");

            VideoStream.RegisterCodec("VP8", () =>
            {
                return new Vp8Codec();
            }, true);

            AudioStream.RegisterCodec("opus", 48000, 2, () =>
            {
                return new OpusCodec();
            }, true);

            DefaultProviders.AndroidContext = this;
        }

        private void SetupCall()
        {
            Signalling Signalling = new Signalling(Constants.WEB_SYNC_SERVER);
            Signalling.Start((error) =>
            {
                if (error != null)
                {
                    // TODO: Handle Errors
                }
            });

            LocalMedia = new LocalMedia();
            LocalMedia.Start(Container, (error) =>
            {
                if (error != null)
                {
                    //TODO: Handle Errors
                }
            });

            var audioStream = new AudioStream(LocalMedia.LocalMediaStream);
            var videoStream = new VideoStream(LocalMedia.LocalMediaStream);
            var conference = new Conference(Constants.ICE_LINK_ADDRESS, new Stream[]
                    {
                        audioStream,
                        videoStream
                    });
            conference.RelayUsername = "test";
            conference.RelayPassword = "pa55w0rd!";

            Signalling.Attach(conference, Constants.SESSION_ID, (error) =>
            {
                if (error != null)
                {
                    // TODO: Handle Errors
                }
            });
        }
    }
}