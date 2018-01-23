using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolXam.Validation
{
	public interface IValidatableBase
	{
		bool IsValidationEnabled { get; set; }

		Validator Errors { get; }

		event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		ReadOnlyDictionary<string, ReadOnlyCollection<string>> GetAllErrors();

		bool ValidateProperties();

		void SetAllErrors(IDictionary<string, ReadOnlyCollection<string>> entityErrors);
	}
}
