using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternObserverViaActionDelegate
{
    static class Program
    {
        static void Main(string[] args)
        {
            //coздать объект класса Thermostat
            Thermostat thermostat = new Thermostat();

            //coздать объект класса Heater установив начальную температуру равную 30 градусов
            Heater heater = new Heater(30);

            //coздать объект класса Cooler установив начальную температуру равную 40 градусов
            Cooler cooler = new Cooler(40);

            //объект класса Heater - подписаться на событие изменения температуры класса Thermostat
            thermostat.TemperatureChanged = heater.Update;

            //объект класса Cooler - подписаться на событие изменения температуры класса Thermostat
            thermostat.TemperatureChanged += cooler.Update;

            //эмуляция изменения температуры объекта класса Thermostat
            thermostat.EmulateTemperatureChange();

            //объект класса Cooler - отписаться от события изменения температуры класса Thermostat
            thermostat.TemperatureChanged -= cooler.Update;

            //эмуляция изменения температуры объекта класса Thermostat на 45 градусов
            thermostat.EmulateTemperatureChange();
        }
    }

    public class Cooler 
    {
        public Cooler(int temperature) => Temperature = temperature;

        public int Temperature { get; private set; }

        public void Update(object sender, EventArgs info)
        {
            var newTemperature = info as TemperatureChangedEventArgs;
            Console.WriteLine(newTemperature.Temperature > Temperature
                      ? $"Cooler: On. Changed:{Math.Abs(newTemperature.Temperature - Temperature)}"
                      : $"Cooler: Off. Changed:{Math.Abs(newTemperature.Temperature - Temperature)}");
        }
    }

    public class Heater 
    {
        public Heater(int temperature) => Temperature = temperature;

        public int Temperature { get; private set; }

        public void Update(object sender, EventArgs info)
        {
            var newTemperature = info as TemperatureChangedEventArgs;
            Console.WriteLine(newTemperature.Temperature < Temperature
                      ? $"Heater: On. Changed:{Math.Abs(newTemperature.Temperature - Temperature)}"
                      : $"Heater: Off. Changed:{Math.Abs(newTemperature.Temperature - Temperature)}");
        }
    }

    public sealed class Thermostat
    {
        private int currentTemperature;

        public Action<object, EventArgs> TemperatureChanged { get; set; }

        private Random random = new Random(Environment.TickCount);

        public Thermostat()
        {
            currentTemperature = 5;
        }

        public int CurrentTemperature
        {
            get => currentTemperature;
            private set
            {
                if (value > currentTemperature)
                {
                    currentTemperature = value;
                    OnTemperatureChanged();
                }
            }
        }

        public void EmulateTemperatureChange()
        {
            this.CurrentTemperature = random.Next(0, 100);
        }

        private void OnTemperatureChanged()
        {
            TemperatureChanged?.Invoke(this, new TemperatureChangedEventArgs(CurrentTemperature));
        }

        public void Register(object observer)
        {
            
        }

        public void Unregister(object observer)
        {
            
        }
    }

    public class TemperatureChangedEventArgs : EventArgs
    {
        public int Temperature { get; set; }

        public TemperatureChangedEventArgs(int temperature)
        {
            Temperature = temperature;
        }
    }
}