﻿using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NMSTestClientUI
{
    /// <summary>
    /// Interaction logic for GetExtentValues.xaml
    /// </summary>
    public partial class GetExtentValues : Window
    {
        public static TestGda testGda;
        public static ModelResourcesDesc resourcesDesc;
        public static Dictionary<ModelCode, List<ModelCode>> propertyIDsByModelCode;
        public static List<long> gids;
        public static ModelCode code;

        public GetExtentValues()
        {
            InitializeComponent();

            testGda = new TestGda();
            resourcesDesc = new ModelResourcesDesc();

            propertyIDsByModelCode = Enum.GetValues(typeof(ModelCode))
                .Cast<ModelCode>()
                .Distinct()
                .ToDictionary(mc => mc, mc => resourcesDesc.GetAllPropertyIds(mc));

            var codes = new List<ModelCode>
            {
                ModelCode.CONTROL,
                ModelCode.CURVE_DATA,
                ModelCode.REACTIVE_CAPABILITY_CURVE,
                ModelCode.SYNCHRONOUS_MACHINE,
                ModelCode.TAP_CHANGER_CONTROL,
                ModelCode.TERMINAL
            };

            ModelCodeComboBox.ItemsSource = codes
                .Select(c => $"{c} - 0x{(long)c:x16}")
                .ToList();
        }

        private void ModelCodeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ModelCodeComboBox.SelectedIndex < 0) return;

            var selectedText = ModelCodeComboBox.SelectedItem.ToString();
            var codeName = selectedText.Split('-')[0].Trim();

            if (Enum.TryParse(codeName, out ModelCode parsedCode))
            {
                code = parsedCode;
                CreateCheckBoxes();
            }
        }

        private void CreateCheckBoxes()
        {
            CheckBoxGrid.Children.Clear();

            var props = propertyIDsByModelCode[code];
            GetExtentValuesButton.Visibility = Visibility.Visible;
            SelectAllCheckBox.Visibility = Visibility.Visible;
            SelectAllCheckBox.IsChecked = false;

            foreach (var prop in props)
            {
                var cb = new CheckBox
                {
                    Content = prop.ToString(),
                    FontSize = 14,
                    FontFamily = new FontFamily("Trebuchet MS"),
                    Margin = new Thickness(0, 2, 0, 2),
                    Visibility = Visibility.Visible
                };
                CheckBoxGrid.Children.Add(cb);
            }
        }

        private List<ModelCode> GetPropertiesForCheckedBoxes()
        {
            if (SelectAllCheckBox.IsChecked == true)
                return propertyIDsByModelCode[code];

            var selectedProps = new List<ModelCode>();

            foreach (var child in CheckBoxGrid.Children)
            {
                if (child is CheckBox cb && cb.IsChecked == true)
                {
                    if (Enum.TryParse(cb.Content.ToString(), out ModelCode prop))
                    {
                        selectedProps.Add(prop);
                    }
                }
            }

            return selectedProps;
        }

        private void GetExtentValuesButton_Click(object sender, RoutedEventArgs e)
        {
            var properties = GetPropertiesForCheckedBoxes();
            gids = testGda.GetExtentValues(code, properties);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"ResourceDescriptions for {code}:");

            foreach (var gid in gids)
            {
                var rd = testGda.GetValues(gid, properties);
                sb.AppendLine();
                sb.AppendLine($"Gid = 0x{rd.Id:x16}");
                sb.AppendLine($"Type = {(DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(rd.Id)}");
                sb.AppendLine("Properties:");

                foreach (var p in rd.Properties)
                {
                    sb.Append($"\t{p.Id} = ");

                    switch (p.Type)
                    {
                        case PropertyType.Float:
                            sb.Append(p.AsFloat());
                            break;
                        case PropertyType.Bool:
                            sb.Append(p.PropertyValue.FloatValue == 1 ? "True" : "False");
                            break;
                        case PropertyType.Byte:
                        case PropertyType.Int32:
                        case PropertyType.Int64:
                        case PropertyType.TimeSpan:
                        case PropertyType.DateTime:
                            sb.Append(p.Id == ModelCode.IDOBJ_GID ? $"0x{p.AsLong():x16}" : p.AsLong().ToString());
                            break;
                        case PropertyType.Reference:
                            sb.Append($"0x{p.AsReference():x16}");
                            break;
                        case PropertyType.String:
                            sb.Append(p.AsString() ?? string.Empty);
                            break;
                        case PropertyType.Int64Vector:
                        case PropertyType.ReferenceVector:
                            var longs = p.AsLongs();
                            sb.Append(longs.Any()
                                ? string.Join(", ", longs.Select(val => $"0x{val:x16}"))
                                : "empty long/reference vector");
                            break;
                        default:
                            throw new Exception("Invalid property type.");
                    }

                    sb.AppendLine();
                }
            }

            ResultsTextBox.Text = sb.ToString();
        }

        private void SelectAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            bool isChecked = SelectAllCheckBox.IsChecked == true;

            foreach (var child in CheckBoxGrid.Children)
            {
                if (child is CheckBox cb)
                    cb.IsChecked = isChecked;
            }
        }

        private void Button_GetValues_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            this.Close();
            window.Show();
        }

        private void Button_GetExtentValues_Click(object sender, RoutedEventArgs e)
        {
            var window = new GetExtentValues();
            this.Close();
            window.Show();
        }

        private void Button_GetRelatedValues_Click(object sender, RoutedEventArgs e)
        {
            var window = new GetRelatedValues();
            this.Close();
            window.Show();
        }
    }
}
