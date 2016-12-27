﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using PickerViewSample.Annotations;

namespace PickerViewSample
{
	internal class MultiModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

		private int[] _numbers;
		public int[] Numbers {
            get { return _numbers; }
            set
            {
                _numbers = value;
                OnPropertyChanged();
            }
        }

        private decimal _value;
        public decimal Value
        {
            get { return _value; }
            set
            {
				if (_value == value)
				{
					return;
				}
				_value = value;
				OnPropertyChanged();
            }
        }

        private int _integerDigit0;
        public int IntegerDigit0
        {
            get { return _integerDigit0; }
            set
            {
                _integerDigit0 = value;
                OnPropertyChanged();
            }
        }

		private int _integerDigit1;
		public int IntegerDigit1
        {
            get { return _integerDigit1; }
            set
            {
                _integerDigit1 = value;
                OnPropertyChanged();
            }
        }

		private int _integerDigit2;
		public int IntegerDigit2
        {
            get { return _integerDigit2; }
            set
            {
                _integerDigit2 = value;
                OnPropertyChanged();
            }
        }

		private int _integerDigit3;
		public int IntegerDigit3
        {
            get { return _integerDigit3; }
            set
            {
                _integerDigit3 = value;
                OnPropertyChanged();
            }
        }

		private int _integerDigit4;
		public int IntegerDigit4
        {
            get { return _integerDigit4; }
            set
            {
                _integerDigit4 = value;
                OnPropertyChanged();
            }
        }

		private int _integerDigit5;
		public int IntegerDigit5
        {
            get { return _integerDigit5; }
            set
            {
                _integerDigit5 = value;
                OnPropertyChanged();
            }
        }

        private int _decimalDigit0;
        public int DecimalDigit0
        {
            get { return _decimalDigit0; }
            set
            {
                _decimalDigit0 = value;
                OnPropertyChanged();
            }
        }

		private int _decimalDigit1;
		public int DecimalDigit1
		{
			get { return _decimalDigit1; }
			set
			{
				_decimalDigit1 = value;
				OnPropertyChanged();
			}
		}

		private int _decimalDigit2;
		public int DecimalDigit2
		{
			get { return _decimalDigit2; }
			set
			{
				_decimalDigit2 = value;
				OnPropertyChanged();
			}
		}

		readonly IList<Action<int>> _intSetters = new List<Action<int>>();
		readonly IList<Func<int>> _intGetters = new List<Func<int>>();

		readonly IList<Action<int>> _decSetters = new List<Action<int>>();
		readonly IList<Func<int>> _decGetters = new List<Func<int>>();

        public MultiModel()
        {
            Numbers = new int[] {0,1,2,3,4,5,6,7,8,9};

            _intSetters.Add(digitValue => IntegerDigit0 = digitValue);
            _intSetters.Add(digitValue => IntegerDigit1 = digitValue);
            _intSetters.Add(digitValue => IntegerDigit2 = digitValue);
            _intSetters.Add(digitValue => IntegerDigit3 = digitValue);
            _intSetters.Add(digitValue => IntegerDigit4 = digitValue);
            _intSetters.Add(digitValue => IntegerDigit5 = digitValue);

            _intGetters.Add(() => IntegerDigit0);
            _intGetters.Add(() => IntegerDigit1);
            _intGetters.Add(() => IntegerDigit2);
            _intGetters.Add(() => IntegerDigit3);
            _intGetters.Add(() => IntegerDigit4);
            _intGetters.Add(() => IntegerDigit5);

			_decSetters.Add(digitValue => DecimalDigit0 = digitValue);
			_decSetters.Add(digitValue => DecimalDigit1 = digitValue);
			_decSetters.Add(digitValue => DecimalDigit2 = digitValue);

			_decGetters.Add(() => DecimalDigit0);
			_decGetters.Add(() => DecimalDigit1);
			_decGetters.Add(() => DecimalDigit2);

			Value = 12345.3M;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
			Debug.WriteLine($"OnPropertyChanged:{propertyName}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

			if (propertyName.StartsWith("IntegerDigit", StringComparison.Ordinal))
            {
				var digitIndex = int.Parse(propertyName.Replace("IntegerDigit", ""));
				UpdateIntDigit(digitIndex, _intGetters[digitIndex].Invoke());
            }
            else if (propertyName.StartsWith("DecimalDigit", StringComparison.Ordinal))
            {
                var digitIndex = int.Parse(propertyName.Replace("DecimalDigit", ""));
                UpdateDecDigit(digitIndex, _decGetters[digitIndex].Invoke());
            }
            else if (propertyName == "Value")
            {
                UpdateValue();
            }
        }

        private void UpdateIntDigit(int digitIndex, int digitValue)
        {
			var numStr = Value.ToString(new string('0', _intGetters.Count) 
			                            + "." 
			                            + new string('0', _decGetters.Count));
			var index = numStr.Length - (digitIndex + 2 + _decGetters.Count);
            var newNum = numStr.Substring(0, index) + digitValue.ToString() +  numStr.Substring(index + 1);

            Value = decimal.Parse(newNum);
        }

        private void UpdateDecDigit(int digitIndex, int digitValue)
        {
            var numStr = Value.ToString("0." + new string('0', _decGetters.Count));
			var index = Value.ToString("0").Length + (digitIndex + 1);
			var newNum = numStr.Substring(0, index) + digitValue.ToString() +  numStr.Substring(index + 1);

            Value = decimal.Parse(newNum);
        }

        private void UpdateValue()
        {
            for (int i = 0; i < _intGetters.Count; i++)
            {
                var digitValue = GetIntDigitValue(i);
                if (digitValue != _intGetters[i].Invoke())
                {
                    _intSetters[i].Invoke(digitValue);
                }
            }

			for (int i = 0; i < _decGetters.Count; i++)
			{
				var digitValue = GetDecDigitValue(i);
				if (digitValue != _decGetters[i].Invoke())
				{
					_decSetters[i].Invoke(digitValue);
				}
			}
		}

        private int GetIntDigitValue(int digitIndex)
		{
			var numStr = Math.Floor(Value).ToString("0");
            var index = numStr.Length - (digitIndex + 1);

            if (index < 0)
            {
                return 0;
            }

            return Convert.ToInt32(numStr.Substring(index, 1));
        }

		private int GetDecDigitValue(int digitIndex)
		{
			var numStr = Value.ToString("0." + new string('0', _decGetters.Count)); // 12.3 -> 12.30
			var index = Value.ToString("0").Length + (digitIndex + 1);

			if (index < 0)
			{
				return 0;
			}

			return Convert.ToInt32(numStr.Substring(index, 1));
		}
	}
}