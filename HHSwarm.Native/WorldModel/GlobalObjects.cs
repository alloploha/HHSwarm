using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.WorldModel
{
    public class GlobalObjects : IGlobalObjectsReceiver
    {
        public bool IsIncrement;

        #region Light
        public Light Light { get; private set; }

        public delegate void LightChangedDelegate(Light previousValue, Light newValue);
        public event LightChangedDelegate LightChanged;

        public void Receive(Light newValue)
        {
            var previousValue = Light;
            Light = newValue;
            LightChanged?.Invoke(previousValue, newValue);
        }
        #endregion

        #region Weather
        public Weather Weather { get; private set; }

        public delegate void WeatherChangedDelegate(Weather previousValue, Weather newValue);
        public event WeatherChangedDelegate WeatherChanged;

        public void Receive(Weather newValue)
        {
            var previousValue = Weather;
            Weather = newValue;
            WeatherChanged?.Invoke(previousValue, newValue);
        }
        #endregion

        #region Sky
        public Sky Sky { get; private set; }

        public delegate void SkyChangedDelegate(Sky previousValue, Sky newValue);
        public event SkyChangedDelegate SkyChanged;

        public void Receive(Sky newValue)
        {
            var previousValue = Sky;
            Sky = newValue;
            SkyChanged?.Invoke(previousValue, newValue);
        }
        #endregion

        #region Astronomy
        public Astronomy Astronomy { get; private set; }

        public delegate void AstronomyChangedDelegate(Astronomy previousValue, Astronomy newValue);
        public event AstronomyChangedDelegate AstronomyChanged;

        public void Receive(Astronomy newValue)
        {
            var previousValue = Astronomy;
            Astronomy = newValue;
            AstronomyChanged?.Invoke(previousValue, newValue);
        }
        #endregion

        #region WorldTime
        public WorldTime WorldTime { get; private set; }

        public delegate void WorldTimeChangedDelegate(WorldTime previousValue, WorldTime newValue);
        public event WorldTimeChangedDelegate WorldTimeChanged;

        public void Receive(WorldTime newValue)
        {
            var previousValue = WorldTime;
            WorldTime = newValue;
            WorldTimeChanged?.Invoke(previousValue, newValue);
        }
        #endregion

        public void ReceiveIncrementFlag(bool value)
        {
            this.IsIncrement = value;
        }

        public void Receive(GlobalObjects other)
        {
            Receive(other.Light);
            Receive(other.Weather);
            Receive(other.Sky);
            Receive(other.Astronomy);
            Receive(other.WorldTime);
        }
    }
}
