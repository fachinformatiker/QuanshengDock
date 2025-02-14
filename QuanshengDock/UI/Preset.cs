﻿using QuanshengDock.General;
using QuanshengDock.User;
using QuanshengDock.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace QuanshengDock.UI
{
    // code by -nicsure- 2024
    // https://www.youtube.com/nicsure

    public class Preset : INotifyPropertyChanged
    {
        public static int Activator { get => 0; set { } }

        private static readonly SavedDictionary backing = new(UserFolder.File("presets.conf"));

        private static readonly ViewModel<double> trigger = VM.Get<double>("Trigger");
        private static readonly ViewModel<double> rxTimeout = VM.Get<double>("RXTimeout");
        private static readonly ViewModel<double> totalTimeout = VM.Get<double>("TotalTimeout");
        private static readonly ViewModel<double> specMid = VM.Get<double>("SpecMid");
        private static readonly ViewModel<double> specStep = VM.Get<double>("SpecStep");
        private static readonly ViewModel<double> specSteps = VM.Get<double>("SpecSteps");
        private static readonly ViewModel<double> specAmp = VM.Get<double>("SpecAmp");
        private static readonly ViewModel<double> specFloor = VM.Get<double>("SpecFloor");
        private static readonly ViewModel<bool> specNorm = VM.Get<bool>("SpecNorm");
        private static readonly ViewModel<ObservableCollection<Preset>> presets = VM.Get<ObservableCollection<Preset>>("MenuCommand");

        public event PropertyChangedEventHandler? PropertyChanged;
        
        public double RXTimeout { get; set; }
        public double TotalTimeout { get; set; }
        public double Trigger { get; set; }
        public double SpecMid { get; set; }
        public double SpecStep { get; set; }
        public double SpecSteps { get; set; }
        public double SpecAmp { get; set; }
        public double SpecFloor { get; set; }
        public bool SpecNorm { get; set; }
        public string Header { get; set; }
        public string Load => $"LoadPreset,{presets.Value.IndexOf(this)}";
        public string Delete => $"DeletePreset,{presets.Value.IndexOf(this)}";
        public string Replace => $"ReplacePreset,{presets.Value.IndexOf(this)}";
        public static ICommand Command => presets;

        public Preset(string name)
        {
            RXTimeout = rxTimeout.Value;
            TotalTimeout = totalTimeout.Value;
            Trigger = trigger.Value;
            SpecMid = specMid.Value;
            SpecStep = specStep.Value;
            SpecSteps = specSteps.Value;
            SpecAmp = specAmp.Value;
            SpecFloor = specFloor.Value;
            SpecNorm = specNorm.Value;
            Header = name;
            presets.Value.Add(this);
            presets.PropertyChanged += Presets_PropertyChanged;
        }

        private Preset()
        {
            Header = string.Empty;
            presets.Value.Add(this);
        }

        private void Presets_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            var pc = PropertyChanged;
            if (pc != null)
            {
                pc.Invoke(this, new(nameof(Load)));
                pc.Invoke(this, new(nameof(Delete)));
                pc.Invoke(this, new(nameof(Replace)));
            }
        }

        static Preset()
        {
            foreach(var header in backing.Keys)
            {
                Preset p = new()
                {
                    Header = header
                };
                Deserialize(p, backing[header]);
            }
        }

        public static void Save()
        {
            backing.Clear();
            foreach(var p in presets.Value)
                backing[p.Header] = Serialize(p);
            backing.Save();
        }

        public static string Serialize(Preset p)
        {
            return
                $"{p.Trigger},{p.RXTimeout},{p.TotalTimeout},{p.SpecMid},{p.SpecStep}," +
                $"{p.SpecSteps},{p.SpecAmp},{p.SpecFloor},{p.SpecNorm}";
        }

        public static void Deserialize(Preset p, string str)
        {
            string[] s = str.Split(',');
            p.Trigger = double.Parse(s[0]);
            p.RXTimeout = double.Parse(s[1]);
            p.TotalTimeout = double.Parse(s[2]);
            p.SpecMid = double.Parse(s[3]);
            p.SpecStep = double.Parse(s[4]);
            p.SpecSteps = double.Parse(s[5]);
            p.SpecAmp = double.Parse(s[6]);
            p.SpecFloor = double.Parse(s[7]);
            p.SpecNorm = bool.Parse(s[8]);
        }
    }
}
