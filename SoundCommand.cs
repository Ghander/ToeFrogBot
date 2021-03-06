﻿using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace ToeFrogBot
{
    public class SoundCommand : Command, IDisposable
    {
        private static string _soundAssetsPath = "assets\\sounds\\";

        public string AudioFile { get; }

        private AudioFileReader AudioReader { get; set; }
        private WaveOutEvent WaveOutDevice { get; set; }

        public SoundCommand(string name)
        {
            this.AudioFile = _soundAssetsPath + name + ".mp3";
        }

        public static bool Exists(string name)
        {
            string soundPath = _soundAssetsPath + name + ".mp3";
            var f = new FileInfo(soundPath);

            return f.Exists;
        }

        public override void Execute()
        {
            using (this.AudioReader = new AudioFileReader(this.AudioFile))
            using (this.WaveOutDevice = new WaveOutEvent())
            {
                this.WaveOutDevice.Init(this.AudioReader);
                this.WaveOutDevice.Play();
                while (this.WaveOutDevice.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        public override void Stop()
        {
            this.WaveOutDevice.Stop();
            this.WaveOutDevice.Dispose();
        }

        public void Dispose()
        {
            Console.WriteLine("Dispose of the SoundCommand");
            this.Stop();
            GC.SuppressFinalize(true);
        }
    }
}