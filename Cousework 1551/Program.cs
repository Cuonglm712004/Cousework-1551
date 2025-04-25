using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Coursework_1551
{
    public class MainForm : Form
    {
        private TextBox txtInput;
        private TextBox txtShift;
        private Button btnEncode;
        private Button btnTestEncoders;
        private ListBox lstEncodedStrings;
        private ComboBox cmbEncodingTechnique;
        private TextBox txtAsciiCodes;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Client size and form properties
            this.ClientSize = new Size(520, 830);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Text = " CONVERSION ALGORITHM PROGRAM";
            this.BackColor = Color.White;

            int formWidth = this.ClientSize.Width;
            int controlWidth = 320;
            int xCenter = (formWidth - controlWidth) / 2;

            Color labelColor = Color.DodgerBlue;
            Color inputBgColor = Color.AliceBlue;
            Color textColor = Color.Navy;
            Font labelFont = new Font("Segoe UI", 10, FontStyle.Bold);

            // ComboBox for encoding selection
            Label lblTechnique = new Label
            {
                Text = " Select Encoding Technique:",
                Location = new Point(xCenter, 20),
                Width = controlWidth,
                ForeColor = labelColor,
                Font = labelFont,
                TextAlign = ContentAlignment.MiddleCenter
            };
            cmbEncodingTechnique = new ComboBox
            {
                Location = new Point(xCenter, 45),
                Width = controlWidth,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = inputBgColor,
                ForeColor = textColor,
                Font = new Font("Segoe UI", 9)
            };
            cmbEncodingTechnique.Items.Add("Caesar Processor");
            cmbEncodingTechnique.Items.Add("Reverse Processor");
            cmbEncodingTechnique.Items.Add("Atbash Processor");
            cmbEncodingTechnique.Items.Add("Mirror Processor");
            cmbEncodingTechnique.Items.Add("Vowel Replacer Processor");

            // Input string
            Label lblInput = new Label
            {
                Text = " Enter CAPITAL letters (A–Z, max 40):",
                Location = new Point(xCenter, 85),
                Width = controlWidth,
                Font = labelFont,
                ForeColor = labelColor,
                TextAlign = ContentAlignment.MiddleCenter
            };
            txtInput = new TextBox
            {
                Location = new Point(xCenter, 110),
                Width = controlWidth,
                BackColor = inputBgColor,
                ForeColor = textColor,
                Font = new Font("Segoe UI", 9)
            };

            // Shift value
            Label lblShift = new Label
            {
                Text = " Enter a number from -25 to 25:",
                Location = new Point(xCenter, 150),
                Width = controlWidth,
                Font = labelFont,
                ForeColor = labelColor,
                TextAlign = ContentAlignment.MiddleCenter
            };
            txtShift = new TextBox
            {
                Location = new Point(xCenter, 175),
                Width = controlWidth,
                BackColor = inputBgColor,
                ForeColor = textColor,
                Font = new Font("Segoe UI", 9)
            };

            // Encode button
            btnEncode = new Button
            {
                Text = " RUN LOCAL INPUT",
                Location = new Point(xCenter, 215),
                Width = controlWidth,
                Height = 40,
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnEncode.Click += btnEncode_Click;

            // Test encoders button
            btnTestEncoders = new Button
            {
                Text = " RUN ALL PROCESSOR",
                Location = new Point(xCenter, 265),
                Width = controlWidth,
                Height = 40,
                BackColor = Color.Green,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            btnTestEncoders.Click += btnTestEncoders_Click;


            // Encoded output list
            lstEncodedStrings = new ListBox
            {
                Location = new Point(xCenter, 310),
                Width = controlWidth,
                Height = 440,
                BackColor = Color.WhiteSmoke,
                ForeColor = textColor,
                Font = new Font("Consolas", 9)
            };

            // ASCII codes output
            txtAsciiCodes = new TextBox
            {
                Location = new Point(xCenter, 760),
                Width = controlWidth,
                Height = 60,
                Multiline = true,
                ReadOnly = true,
                BackColor = Color.WhiteSmoke,
                ForeColor = textColor,
                Font = new Font("Segoe UI", 9)
            };

            this.Controls.AddRange(new Control[]
            {
                lblTechnique, cmbEncodingTechnique, lblInput, txtInput,
                lblShift, txtShift, btnEncode, btnTestEncoders,
                lstEncodedStrings, txtAsciiCodes
            });
        }

        private void btnEncode_Click(object sender, EventArgs e)
        {
            lstEncodedStrings.Items.Clear();
            txtAsciiCodes.Clear();

            string input = txtInput.Text;
            if (!IsValidInputString(input))
            {
                MessageBox.Show("Please enter only capital letters (A–Z) and no more than 40 characters.",
                    "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? shift = null;
            if (cmbEncodingTechnique.SelectedIndex == 0)
            {
                if (!int.TryParse(txtShift.Text, out int parsedShift) || !IsValidShiftValue(parsedShift))
                {
                    MessageBox.Show("Shift must be a number between -25 and 25.",
                        "Invalid Shift", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                shift = parsedShift;
            }

            if (cmbEncodingTechnique.SelectedIndex < 0)
            {
                MessageBox.Show("Please select an encoding technique.",
                    "No Technique Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            EncodeProcessorBase encoder;
            string techniqueName = cmbEncodingTechnique.SelectedItem.ToString();
            string encoded = "", decoded = "";

            switch (cmbEncodingTechnique.SelectedIndex)
            {
                case 0: // Caesar
                    encoder = new CaesarProcessor(input, shift);
                    encoded = encoder.Encode();
                    var decoder = new CaesarProcessor(encoded, -shift);
                    decoded = decoder.Encode();

                    break;

                case 1: // Reverse
                    encoder = new ReverseProcessor(input);
                    encoded = encoder.Encode();
                    decoded = new ReverseProcessor(encoded).Encode();

                    break;

                case 2: // Atbash
                    encoder = new AtbashProcessor(input);
                    encoded = encoder.Encode();
                    decoded = new AtbashProcessor(encoded).Encode();

                    break;

                case 3: // Mirror
                    encoder = new MirrorProcessor(input);
                    encoded = encoder.Encode();
                    decoded = "(N/A)";

                    break;

                case 4: // Vowel Replacer
                    encoder = new VowelReplacerProcessor(input);
                    encoded = encoder.Encode();
                    decoded = "(N/A)";

                    break;

                default:
                    MessageBox.Show("Unsupported technique.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            // Show formatted result like in test
            lstEncodedStrings.Items.Add($"=== {techniqueName} ===");
            lstEncodedStrings.Items.Add($"Original : {input}");
            lstEncodedStrings.Items.Add($"Encoded  : {encoded}");
            if (decoded != "")
                lstEncodedStrings.Items.Add($"Decoded  : {decoded}");
            lstEncodedStrings.Items.Add("");

            // Display ASCII codes
            StringProcessing processor = new StringProcessing(encoder);
            processor.ShowAsciiCodes(txtAsciiCodes);

            // Save to DB
            DatabaseHelper.SaveEncodedString(input, encoded, techniqueName); // Save for selected technique

            // Additional: Save for all processors
            SaveForAllProcessors(input);
        }

        private void SaveForAllProcessors(string input)
        {
            string caesarEncoded = new CaesarProcessor(input, 5).Encode();
            string reverseEncoded = new ReverseProcessor(input).Encode();
            string atbashEncoded = new AtbashProcessor(input).Encode();
            string mirrorEncoded = new MirrorProcessor(input).Encode();
            string vowelReplacerEncoded = new VowelReplacerProcessor(input).Encode();


            DatabaseHelper.SaveEncodedStringWithProcessors(
                input, caesarEncoded, reverseEncoded, atbashEncoded, mirrorEncoded, vowelReplacerEncoded
            );
        }

        private void SaveEncodedResults(string input, string encoded)
        {
            string caesarEncoded = null, reverseEncoded = null, atbashEncoded = null, mirrorEncoded = null, vowelReplacerEncoded = null;

            // Save Caesar result
            int shift = 5;
            CaesarProcessor caesarProcessor = new CaesarProcessor(input, shift);
            caesarEncoded = caesarProcessor.Encode();

            // Save Reverse result
            ReverseProcessor reverseProcessor = new ReverseProcessor(input);
            reverseEncoded = reverseProcessor.Encode();

            // Save Atbash result
            AtbashProcessor atbashProcessor = new AtbashProcessor(input);
            atbashEncoded = atbashProcessor.Encode();

            // Save Mirror result
            MirrorProcessor mirrorProcessor = new MirrorProcessor(input);
            mirrorEncoded = mirrorProcessor.Encode();

            // Save Vowel Replacer result
            VowelReplacerProcessor vowelReplacerProcessor = new VowelReplacerProcessor(input);
            vowelReplacerEncoded = vowelReplacerProcessor.Encode();
        }


        private void btnTestEncoders_Click(object sender, EventArgs e)
        {
            lstEncodedStrings.Items.Clear();
            txtAsciiCodes.Clear();

            string original = "HELLOWORLD";

            // Caesar Cipher Test
            int shift = 5;
            CaesarProcessor caencoder = new CaesarProcessor(original, shift);
            string encoded = caencoder.Encode();
            CaesarProcessor cadecoder = new CaesarProcessor(encoded, -shift);
            string decoded = cadecoder.Encode();

            lstEncodedStrings.Items.Add("=== Caesar Cipher Test ===");
            lstEncodedStrings.Items.Add($"Original: {original}");
            lstEncodedStrings.Items.Add($"Encoded : {encoded}");
            lstEncodedStrings.Items.Add($"Decoded : {decoded}");
            lstEncodedStrings.Items.Add($"Result: {(original == decoded ? "PASS" : "FAIL")}");
            lstEncodedStrings.Items.Add("");

            // Atbash Encoding Test
            AtbashProcessor atbashEncoder = new AtbashProcessor(original);
            string atbashEncoded = atbashEncoder.Encode();
            AtbashProcessor atbashDecoder = new AtbashProcessor(atbashEncoded);
            string atbashDecoded = atbashDecoder.Encode();

            lstEncodedStrings.Items.Add("=== Atbash Encoding Test ===");
            lstEncodedStrings.Items.Add($"Original: {original}");
            lstEncodedStrings.Items.Add($"Atbash Encoded: {atbashEncoded}");
            lstEncodedStrings.Items.Add($"Atbash Decoded: {atbashDecoded}");
            lstEncodedStrings.Items.Add($"Result: {(original == atbashDecoded ? "PASS" : "FAIL")}");
            lstEncodedStrings.Items.Add("");

            // Mirror Encoding Test
            MirrorProcessor mirrorEncoder = new MirrorProcessor(original);
            string mirrorEncoded = mirrorEncoder.Encode();
            MirrorProcessor mirrorDecoder = new MirrorProcessor(mirrorEncoded);
            string mirrorDecoded = mirrorDecoder.Encode();

            lstEncodedStrings.Items.Add("=== Mirror Encoding Test ===");
            lstEncodedStrings.Items.Add($"Original: {original}");
            lstEncodedStrings.Items.Add($"Mirror Encoded: {mirrorEncoded}");
            lstEncodedStrings.Items.Add($"Mirror Decoded: {mirrorDecoded}");
            lstEncodedStrings.Items.Add($"Result: {(original == mirrorDecoded ? "PASS" : "FAIL")}");
            lstEncodedStrings.Items.Add("");

            // Vowel Replacer Encoding Test
            try
            {
                VowelReplacerProcessor vowelReplacerEncoder = new VowelReplacerProcessor(original);
                string vowelReplaced = vowelReplacerEncoder.Encode();
                VowelReplacerProcessor vowelReplacerDecoder = new VowelReplacerProcessor(vowelReplaced);
                string vowelRestored = vowelReplacerDecoder.Encode();

                lstEncodedStrings.Items.Add("=== Vowel Replacer Encoding Test ===");
                lstEncodedStrings.Items.Add($"Original: {original}");
                lstEncodedStrings.Items.Add($"Vowel Replaced: {vowelReplaced}");
                lstEncodedStrings.Items.Add($"Vowel Restored: (N/A)");
                lstEncodedStrings.Items.Add($"Result: N/A (One-way encoding)");
                lstEncodedStrings.Items.Add("");
            }
            catch (Exception ex)
            {
                lstEncodedStrings.Items.Add($"Error in VowelReplacerProcessor: {ex.Message}");
            }

            // Reverse Encoding Test
            ReverseProcessor revEncoder = new ReverseProcessor(original);
            string reversed = revEncoder.Encode();
            ReverseProcessor revDecoder = new ReverseProcessor(reversed);
            string restored = revDecoder.Encode();

            lstEncodedStrings.Items.Add("=== Reverse Encoding Test ===");
            lstEncodedStrings.Items.Add($"Original: {original}");
            lstEncodedStrings.Items.Add($"Reversed: {reversed}");
            lstEncodedStrings.Items.Add($"Restored: {restored}");
            lstEncodedStrings.Items.Add($"Result: {(original == restored ? "PASS" : "FAIL")}");
        }

        private bool IsValidInputString(string input) =>
            input.All(c => c >= 'A' && c <= 'Z') && input.Length <= 40;

        private bool IsValidShiftValue(int shift) => shift >= -25 && shift <= 25;

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    // Abstract EncodeProcessorBase
    public abstract class EncodeProcessorBase
    {
        private string _inputString;
        protected string InputString
        {
            get => _inputString;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 40 || value.Any(c => (c < 'A' || c > 'Z') && c != '*'))
                    throw new ArgumentException("Invalid input string.");
                _inputString = value;
            }
        }

        private int? _shift;
        protected int? Shift
        {
            get => _shift;
            set => _shift = value;
        }

        public string EncodedString { get; protected set; }

        public EncodeProcessorBase(string inputString)
        {
            InputString = inputString;
            _shift = null;
        }

        public EncodeProcessorBase(string inputString, int shift) : this(inputString)
        {
            _shift = shift;
        }

        public abstract string Encode();

        public int[] InputCode() => InputString.Select(c => (int)c).ToArray();
        public int[] OutputCode() => EncodedString.Select(c => (int)c).ToArray();
    }

    public class CaesarProcessor : EncodeProcessorBase
    {
        public CaesarProcessor(string inputString, int shift) : base(inputString, shift) { }

        public CaesarProcessor(string inputString, int? shift) : base(inputString)
        {
        }

        public override string Encode()
        {
            var result = new StringBuilder();
            foreach (char c in InputString)
                result.Append((char)(((c - 'A' + Shift + 26) % 26) + 'A'));
            EncodedString = result.ToString();
            return EncodedString;
        }
    }

    public class ReverseProcessor : EncodeProcessorBase
    {
        public ReverseProcessor(string inputString) : base(inputString, 0) { }

        public override string Encode()
        {
            var arr = InputString.ToCharArray();
            Array.Reverse(arr);
            EncodedString = new string(arr);
            return EncodedString;
        }
    }

    public class AtbashProcessor : EncodeProcessorBase
    {
        public AtbashProcessor(string inputString) : base(inputString, 0) { }

        public override string Encode()
        {
            var sb = new StringBuilder();

            foreach (char c in InputString)
            {
                if (char.IsUpper(c))
                {
                    sb.Append((char)('Z' - (c - 'A')));
                }
                else if (char.IsLower(c))
                {
                    sb.Append((char)('z' - (c - 'a')));
                }
                else
                {
                    sb.Append(c);
                }
            }

            EncodedString = sb.ToString();
            return EncodedString;
        }
    }

    public class MirrorProcessor : EncodeProcessorBase
    {
        public MirrorProcessor(string inputString) : base(inputString, 0) { }

        public override string Encode()
        {
            {
                var reversed = new string(InputString.Reverse().ToArray());
                EncodedString = InputString + reversed;
                return EncodedString;
            }
        }
    }

    public class VowelReplacerProcessor : EncodeProcessorBase
    {
        public VowelReplacerProcessor(string inputString) : base(inputString, 0) { }

        public override string Encode()
        {
            {
                var sb = new StringBuilder();
                string vowels = "AEIOUaeiou";

                foreach (char c in InputString)
                {
                    if (vowels.Contains(c))
                        sb.Append('*');
                    else
                        sb.Append(c);
                }

                EncodedString = sb.ToString();
                return EncodedString;

            }
        }
    }


    public class StringProcessing
    {
        private readonly EncodeProcessorBase _encoder;

        public StringProcessing(EncodeProcessorBase encoder) => _encoder = encoder;

        public string ProcessEncoding() => _encoder.Encode();

        public void ShowAsciiCodes(TextBox txt)
        {
            var input = FormatAsciiCodes(_encoder.InputCode(), "Input");
            var output = FormatAsciiCodes(_encoder.OutputCode(), "Encoded");
            txt.Text = input + "\n\n" + output;
        }

        private string FormatAsciiCodes(int[] codes, string label)
        {
            var sb = new StringBuilder($"{label} ASCII Codes:\n");
            for (int i = 0; i < codes.Length; i++)
            {
                sb.Append(codes[i].ToString().PadLeft(4));
                if ((i + 1) % 10 == 0 || i == codes.Length - 1) sb.AppendLine();
            }
            return sb.ToString();
        }
    }

    public static class DatabaseHelper
    {
        private static readonly string _connectionString = "Server=localhost;Database=EncodingDB;Uid=root;Pwd=root;";

        public static void SaveEncodedStringWithProcessors(string originalString, string caesarEncoded, string reverseEncoded,
        string atbashEncoded, string mirrorEncoded, string vowelReplacerEncoded)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO EncodedStrings (original_string, encoded_string, encoded_reverse, 
                          encoded_atbash, encoded_mirror, encoded_vowel_replacer) 
                     VALUES (@original_string, @caesarEncoded, @reverseEncoded, 
                             @atbashEncoded, @mirrorEncoded, @vowelReplacerEncoded)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@original_string", originalString);
                    command.Parameters.AddWithValue("@caesarEncoded", caesarEncoded);
                    command.Parameters.AddWithValue("@reverseEncoded", reverseEncoded);
                    command.Parameters.AddWithValue("@atbashEncoded", atbashEncoded);
                    command.Parameters.AddWithValue("@mirrorEncoded", mirrorEncoded);
                    command.Parameters.AddWithValue("@vowelReplacerEncoded", vowelReplacerEncoded);

                    command.ExecuteNonQuery();
                }
            }

        }

        public static void DisplayAllEncodedStrings()
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT id, original_string, encoded_string, created_at FROM EncodedStrings";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["id"]}, Original: {reader["original_string"]}, Encoded: {reader["encoded_string"]}, Created At: {reader["created_at"]}");
                        }
                    }
                }
            }
        }



        public static void DisplayAllEncodedStringss()
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT encoded_string FROM EncodedStrings";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader.GetString(0));
                        }
                    }
                }
            }
        }

        internal static void SaveEncodedString(string input, string encoded, string technique)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                string query = @"
                INSERT INTO EncodedStrings (original_string, encoded_string, technique, created_at) 
                VALUES (@original_string, @encoded_string, @technique, NOW())";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@original_string", input);
                    cmd.Parameters.AddWithValue("@encoded_string", encoded);
                    cmd.Parameters.AddWithValue("@technique", technique);
                }
            }
        }

    }
}