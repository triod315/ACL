using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;

namespace ACApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            FindViewById<Button>(Resource.Id.button1).Click += aplayOnClick;

            FindViewById<Button>(Resource.Id.button2).Click += findOnClick;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void aplayOnClick(object sender, EventArgs e) 
        {
            int count = int.Parse(FindViewById<EditText>(Resource.Id.editText1).Text);

            LinearLayout yourFormLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout1);

            var parameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

            //add some parameters
            parameters.SetMargins(20, 20, 20, 10);
            parameters.Width = 80;
            yourFormLayout.RemoveAllViews();

            for (int i = 0; i < count; i++) { 
                //create new element
                EditText letterTextEdit = new EditText(this);
                letterTextEdit.Text = "";
                /*button.SetBackgroundColor(Android.Graphics.Color.);
                button.SetTextColor(Android.Graphics.Color.White);*/
                letterTextEdit.LayoutParameters = parameters;
                letterTextEdit.Text = "*";

                letterTextEdit.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(1) });

                letterTextEdit.Id = i;

                //Add the button
                yourFormLayout.AddView(letterTextEdit);
            }


        }

        public void findOnClick(object sender, EventArgs e) 
        {
            TextView result = FindViewById<TextView>(Resource.Id.result);
            result.Text = findWords();
        }


        private string getRegExpr() 
        {
            string result="";
            int count = int.Parse(FindViewById<EditText>(Resource.Id.editText1).Text);

            for (int i = 0; i < count; i++) 
            {
                result += FindViewById<EditText>(i).Text;
            }
            result=result.Replace('*', '.');

            return result;
        }
        private string findWords() 
        {
            string result = "";

            string text;
            
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ACApp.EN_wordlist";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }

            text = Regex.Replace(text, @"\r\n", " ");

            var regExpression = $"{getRegExpr()}\\s";


            Regex regex = new Regex(regExpression);
            MatchCollection matches = regex.Matches(text);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                    result += "\n" + match.Value;
            }
            else
            {
                result = "Not found";
            }

            return result;
        }
    }
}
