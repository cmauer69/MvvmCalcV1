using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Windows;
using System.Text.RegularExpressions;


namespace MvvmCalcV1.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        string currentEntry = "0";
        string historyString = "";
        public string CurrentEntry
        {
            set { SetProperty(ref currentEntry, value); }
            get { return currentEntry; }
        }

        public string HistoryString { set { SetProperty(ref historyString, value); } get { return historyString; } }
        public ICommand ClearCommand { set; get; }
        public ICommand ClearEntryCommand { set; get; }
        public ICommand BackspaceCommand { set; get; }
        public ICommand NumericCommand { set; get; }
        public ICommand DecimalPointCommand { set; get; }
        public ICommand AddCommand { set; get; }
        public ICommand EqualsCommand { set; get; }
        public ICommand MultiplyCommand { set; get; }
        public ICommand DivideCommand { set; get; }
        public ICommand SubtractCommand { set; get; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
            {
                return false;
            }
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }
        }

 

        public T GetDictionaryEntry<T>(IDictionary<string, object> dictionary, string key, T defaultvalue)
        {
            if (dictionary.ContainsKey(key))
            {
                return (T)dictionary[key];
            }
            return defaultvalue;
        }

    }
    public class MvvmCalcV1ViewModel : ViewModelBase
    {
        bool isSumDisplayed = false;
        double accumulatedSum = 0;

        public MvvmCalcV1ViewModel()
        {

            ClearCommand = new Command(
              execute: () =>
              {
                  HistoryString = "";
                  accumulatedSum = 0;
                  CurrentEntry = "0";
                  isSumDisplayed = false;
                  RefreshCanExecutes();
              });

            ClearEntryCommand = new Command(
              execute: () =>
            {
                CurrentEntry = "";
                isSumDisplayed = false;
                RefreshCanExecutes();
            });

            BackspaceCommand = new Command(
              execute: () =>
            {
                CurrentEntry = CurrentEntry.Substring(0, CurrentEntry.Length - 1);
                if (CurrentEntry.Length == 0)
                {
                    CurrentEntry = "0";
                }
                RefreshCanExecutes();
            },
              canExecute: () =>
            {
                return !isSumDisplayed && (CurrentEntry.Length > 1 || CurrentEntry[0] != '0');
            });

            NumericCommand = new Command<string>(
              execute: (string parameter) =>
            {
                if (isSumDisplayed || CurrentEntry == "0")
                    CurrentEntry = parameter;
                else CurrentEntry += parameter;

                isSumDisplayed = false;
                String pattern = @"([-+*/])";
            //if the last character is an operator, then add the currententry
            if (Regex.IsMatch(HistoryString, pattern))
                {
                    HistoryString = HistoryString + CurrentEntry;
                }
                else
                {
                    HistoryString = CurrentEntry;
                }
                

                RefreshCanExecutes();
            },
              canExecute: (string parameter) =>
              {
                  return isSumDisplayed || CurrentEntry.Length < 16;
              });

            DecimalPointCommand = new Command(
                execute: () =>
            {
                if (isSumDisplayed)
                    CurrentEntry = "0.";
                else
                    CurrentEntry += ".";
                isSumDisplayed = false;
                RefreshCanExecutes();
            },
                canExecute: () =>
            {
                return isSumDisplayed || !CurrentEntry.Contains(".");
            });

            AddCommand = new Command(
                execute: () =>
            {
                double value = Double.Parse(CurrentEntry);
                HistoryString += " + ";
                //accumulatedSum += value;
                //CurrentEntry = accumulatedSum.ToString();
                isSumDisplayed = true;
                RefreshCanExecutes();
            },
                canExecute: () =>
            {
                return !isSumDisplayed;
            });

            MultiplyCommand = new Command(
                execute: () =>
                {
                    double value = Double.Parse(CurrentEntry);
                    HistoryString += " * ";

                    //accumulatedSum = accumulatedSum * value;
                    //CurrentEntry = accumulatedSum.ToString();

                    isSumDisplayed = true;
                    RefreshCanExecutes();
                },
                canExecute: () =>
                {
                    return !isSumDisplayed;
                });

            DivideCommand = new Command(
                execute: () =>
                {
                    double value = Double.Parse(CurrentEntry);
                    //HistoryString += value.ToString() + "/";
                    HistoryString += "/";

                    //accumulatedSum = accumulatedSum / value;
                    //CurrentEntry = accumulatedSum.ToString();
                    //CurrentEntry = HistoryString;

                    isSumDisplayed = true;
                    RefreshCanExecutes();
                },
                canExecute: () =>
                {
                    return !isSumDisplayed;
                });

            SubtractCommand = new Command(
                execute: () =>
                {
                    double value = Double.Parse(CurrentEntry);
                                //HistoryString += value.ToString() + "/";
                                HistoryString += "-";

                                //accumulatedSum = accumulatedSum / value;
                                //CurrentEntry = accumulatedSum.ToString();
                                //CurrentEntry = HistoryString;

                                isSumDisplayed = true;
                    RefreshCanExecutes();
                },
                canExecute: () =>
                {
                    return !isSumDisplayed;
                });

            EqualsCommand = new Command(
                execute: () =>
                {
                    String displayText = "";
                    displayText = HistoryString;
                    String[] expressions = new String[] { displayText };
                    decimal total = 0.0000000000m;
                    decimal value1 = 0.0000000000m;
                    decimal value2 = 0.0000000000m;

                    String pattern = @"([-+*/])";
                    string[] substrings = Regex.Split(displayText, pattern);    // Split on hyphens
                    foreach (var expression in expressions)
                        foreach (Match m in Regex.Matches(expression, pattern))
                        {
                            Decimal.TryParse(substrings[0], out value1);
                            Decimal.TryParse(substrings[2], out value2);
                            switch (substrings[1])
                            {
                                case "+":
                                    total = value1 + value2;
                                    total = Math.Round(total, 15);
                                    displayText = total.ToString();
                                    break;
                                case "-":
                                    total = value1 - value2;
                                    total = Math.Round(total, 15);
                                    displayText = total.ToString();
                                    break;
                                case "*":
                                    total = value1 * value2;
                                    total = Math.Round(total, 15);
                                    displayText = total.ToString();
                                    break;
                                case "/":
                                    total = value1 / value2;
                                    total = Math.Round(total, 15);
                                    displayText = total.ToString();
                                    break;
                            }


                        }

                    CurrentEntry = displayText;


                },
                canExecute: () =>
                {
                    return !isSumDisplayed;
                });

        }

        void RefreshCanExecutes()
        {
            ((Command)BackspaceCommand).ChangeCanExecute();
            ((Command)NumericCommand).ChangeCanExecute();
            ((Command)DecimalPointCommand).ChangeCanExecute();
            ((Command)AddCommand).ChangeCanExecute();
            ((Command)MultiplyCommand).ChangeCanExecute();
            ((Command)DivideCommand).ChangeCanExecute();
            ((Command)SubtractCommand).ChangeCanExecute();
        }

        public void SaveState(IDictionary<string, object> dictionary)
        {
            dictionary["CurrentEntry"] = CurrentEntry;
            dictionary["HistoryString"] = HistoryString;
            dictionary["isSumDisplayed"] = isSumDisplayed;
            dictionary["accumulatedSum"] = accumulatedSum;

        }

        public void RestoreState(IDictionary<string, object> dictionary)
        {
            CurrentEntry = GetDictionaryEntry(dictionary, "CurrentEntry", "0");
            HistoryString = GetDictionaryEntry(dictionary, "HistoryString", "");
            isSumDisplayed = GetDictionaryEntry(dictionary, "isSumDisplayed", false);
            accumulatedSum = GetDictionaryEntry(dictionary, "accumulatedSum", 0.0);

            RefreshCanExecutes();
        }


    }
}

