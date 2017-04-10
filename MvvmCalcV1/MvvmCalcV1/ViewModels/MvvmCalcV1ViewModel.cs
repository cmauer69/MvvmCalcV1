using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


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
                CurrentEntry = "0";
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
                HistoryString += value.ToString() + " + ";
                accumulatedSum += value;
                CurrentEntry = accumulatedSum.ToString();
                isSumDisplayed = true;
                RefreshCanExecutes();
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

