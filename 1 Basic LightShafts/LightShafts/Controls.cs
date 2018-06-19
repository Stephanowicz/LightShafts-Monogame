using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightShafts
{
    public partial class Controls : Form
    {
        bool init;
        Game1 _game1;
        public Controls(Game1 game1)
        {
            init = true;
            InitializeComponent();
            _game1 = game1;

            trackBarLightPosX.Value = (int)(_game1.LightMapPosition.X * 1000 );
            trackBarLightPosY.Value = (int)(_game1.LightMapPosition.Y * 1000 );
            trackBarLightShaftExposure.Value = (int)(_game1.LightShaftExposure * 1000);
            trackBarLightShaftDecay.Value = (int)(_game1.LightShaftDecay * 1000);
            trackBarLightShaftDensity.Value = (int)(_game1.LightShaftDensity * 1000);
            trackBarLightShaftWeight.Value = (int)(_game1.LightShaftWeight * 1000);
            trackBarGearExposure.Value = (int)(_game1.ModelExposure * 1000);
            trackBarLuminanceThreshold.Value = (int)(_game1.LuminanceThreshold * 1000);
            trackBarLuminanceScaleFactor.Value = (int)(_game1.LuminanceScaleFactor * 1000);
            trackBarLightMapOffsetX.Value = (int)(_game1.LightMapOffset.X * 1000);
            trackBarLightMapOffsetY.Value = (int)(_game1.LightMapOffset.Y * 1000);
            trackBarFlareTexDivisor.Value = (int)(_game1.texFactor * 100);
            init = false;
        }

        private void trackBarLightPos_Scroll(object sender, EventArgs e)
        {
            if(!init)
            _game1.LightMapPosition = new Microsoft.Xna.Framework.Vector2((float)trackBarLightPosX.Value / 1000f , (float)trackBarLightPosY.Value / 1000f );
        }
        private void trackBarLightShaftExposure_Scroll(object sender, EventArgs e)
        {
            if (!init) _game1.LightShaftExposure = (float)trackBarLightShaftExposure.Value / 1000f;
        }

        private void trackBarLightShaftDecay_Scroll(object sender, EventArgs e)
        {
           if (!init)  _game1.LightShaftDecay = (float)trackBarLightShaftDecay.Value / 1000f;

        }

        private void trackBarLightShaftDensity_Scroll(object sender, EventArgs e)
        {
            if (!init) _game1.LightShaftDensity = (float)trackBarLightShaftDensity.Value / 1000f;

        }

        private void trackBarLightShaftWeight_Scroll(object sender, EventArgs e)
        {
            if (!init) _game1.LightShaftWeight = (float)trackBarLightShaftWeight.Value / 1000f;

        }

        private void trackBarGearExposure_Scroll(object sender, EventArgs e)
        {
            if (!init) _game1.ModelExposure = (float)trackBarGearExposure.Value / 1000f;

        }

        private void trackBarLuminanceThreshold_Scroll(object sender, EventArgs e)
        {
            if (!init) _game1.LuminanceThreshold = (float)trackBarLuminanceThreshold.Value / 1000f;

        }

        private void trackBarLuminanceScaleFactor_Scroll(object sender, EventArgs e)
        {
            if (!init) _game1.LuminanceScaleFactor = (float)trackBarLuminanceScaleFactor.Value / 1000f;

        }

        private void trackBarLightMapOffset_Scroll(object sender, EventArgs e)
        {
            if(!init)
            _game1.LightMapOffset = new Microsoft.Xna.Framework.Vector2((float)trackBarLightMapOffsetX.Value / 1000f, (float)trackBarLightMapOffsetY.Value / 1000f);

        }

        private void trackBarFlareTexDivisor_Scroll(object sender, EventArgs e)
        {
            if (!init) _game1.texFactor = (float)trackBarFlareTexDivisor.Value / 100f;
        }
    }
}
