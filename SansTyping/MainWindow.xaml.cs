using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SansTyping {
    public partial class MainWindow : Window {
        private MediaPlayer[] players;
        private int index;

        private int lastHit;

        public MainWindow() {
            InitializeComponent();
            Hook.KeyboardHook.KeyDown += KeyboardHook_KeyDown;
            Hook.KeyboardHook.KeyUp += KeyboardHook_KeyUp;
            Hook.KeyboardHook.HookStart();

            players = new MediaPlayer[5];
            var audioPath = new Uri($"file:///{Path.Combine(Directory.GetCurrentDirectory(), "SansSpeak.wav")}");
            for (var i = 0; i < players.Length; i++) {
                players[i] = new MediaPlayer();
                players[i].Open(audioPath);
            }
        }

        ~MainWindow() {
            Hook.KeyboardHook.HookEnd();
        }

        private bool KeyboardHook_KeyDown(int vkCode) {
            if (lastHit == vkCode) {
                return true;
            }
            lastHit = vkCode;
            Play();
            return true;
        }

        private bool KeyboardHook_KeyUp(int vkCode) {
            if (lastHit == vkCode) {
                lastHit = -1;
            }
            return true;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) {
                this.DragMove();
            }
            else {
                Play();
            }
        }

        private void Play() {
            players[index].Stop();
            players[index].Play();
            index = (index + 1) % players.Length;
        }
    }
}
