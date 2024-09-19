using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomAircraftTemplateJAS39
{
    class DashVertGauge : DashGauge
    {
        public MeasurementManager measures;
        public FlightInfo info;

        public override float GetMeteredValue()
        {
            //FlightLogger.Log(measures.ConvertedVerticalSpeed(info.verticalSpeed).ToString());
            return measures.ConvertedVerticalSpeed(info.verticalSpeed) / 4000f;
        }
    }
}
