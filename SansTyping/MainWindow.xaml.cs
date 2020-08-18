using System.Media;
using System.Windows;
using System.Windows.Input;

namespace SansTyping {
    public partial class MainWindow : Window {
        private SoundPlayer player;

        public MainWindow() {
            InitializeComponent();
            Hook.KeyboardHook.KeyDown += KeyboardHook_KeyDown;
            Hook.KeyboardHook.HookStart();
            player = new SoundPlayer("SansSpeak.wav");
        }

        ~MainWindow() {
            Hook.KeyboardHook.HookEnd();
        }

        private bool KeyboardHook_KeyDown(int vkCode) {
            player.Play();
            return true;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) {
                this.DragMove();
            }
            else {
                player.Play();
            }
        }
    }
}
