using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using FactionLife.Server.Model;

namespace FactionLife.Server.Services.MedicMapL
{
    public class MedicMap : Script
    {
        public MedicMap()
        {
            MedicSecond();
        }

        private void MedicSecond()
        {
            API.createObject(862871082, new Vector3(210.234619, -235.136566, 52.8701096), new Vector3(0, -0, 159.351563), 0);
            API.createObject(427753832, new Vector3(245.965149, -265.999451, 48.1363678), new Vector3(0, -0, -179.723953), 0);
            API.createObject(1317858860, new Vector3(267.271301, -264.692383, 52.7884064), new Vector3(5.08888721e-13, 1.06813764e-06, -102.339256), 0);
            API.createObject(1317858860, new Vector3(249.242889, -287.624695, 52.7221107), new Vector3(1.75562709e-05, 89.999939, 70.5486526), 0);
            API.createObject(1317858860, new Vector3(257.375885, -285.058716, 52.8015137), new Vector3(-1.49011257e-08, 7.95138627e-15, -151.451767), 0);
            API.createObject(1317858860, new Vector3(247.676117, -288.806824, 52.7978554), new Vector3(-1.49010955e-08, 1.51076339e-14, -169.364761), 0);
            API.createObject(1317858860, new Vector3(263.992706, -275.597717, 52.80019), new Vector3(-1.49011239e-08, 9.14409421e-15, -110.806419), 0);
            API.createObject(1317858860, new Vector3(263.902405, -268.951141, 52.7229805), new Vector3(1.77024885e-05, 89.9999466, 160.136993), 0);
            API.createObject(1317858860, new Vector3(244.589081, -277.814117, 52.6871567), new Vector3(1.75562072e-05, 89.999939, -19.9226456), 0);
            API.createObject(1317858860, new Vector3(239.763916, -268.41394, 52.6851349), new Vector3(1.75561599e-05, 89.999939, 68.1674805), 0);
            API.createObject(1171954070, new Vector3(272.898438, -266.578461, 59.5546761), new Vector3(0, 0, -71.6856537), 0);
            API.createObject(726001049, new Vector3(272.062408, -271.856506, 59.550087), new Vector3(-1.11955519e-12, 1.77885374e-06, 13.1696262), 0);
            API.createObject(1276664810, new Vector3(254.991653, -284.758759, 54.0537109), new Vector3(0, -0, 173.640823), 0);
            API.createObject(427753832, new Vector3(245.965149, -265.999451, 48.1363678), new Vector3(0, -0, -179.723953), 0);
            API.createObject(1317858860, new Vector3(244.589081, -277.814117, 52.6871567), new Vector3(1.75562072e-05, 89.999939, -19.9226456), 0);
            API.createObject(1317858860, new Vector3(244.589081, -277.814117, 52.6871567), new Vector3(1.75562072e-05, 89.999939, -19.9226456), 0);
            API.createObject(1317858860, new Vector3(263.902405, -268.951141, 52.7229805), new Vector3(1.77024867e-05, 89.9999466, 160.136993), 0);
            API.createObject(1317858860, new Vector3(247.676117, -288.806824, 52.7978554), new Vector3(-1.49010955e-08, 1.5306419e-14, -169.364746), 0);
            API.createObject(551699682, new Vector3(248.466431, -269.932739, 52.9337654), new Vector3(0, -0, 99.0416412), 0);
            API.createObject(272384846, new Vector3(-328.794128, -133.336914, 38.0136681), new Vector3(0.0331669077, 0.458605021, 142.250793), 0);
            API.createObject(272384846, new Vector3(254.240692, -283.584381, 52.9993515), new Vector3(-25.9853363, 0.430438131, 161.281433), 0);
        }
    }
}