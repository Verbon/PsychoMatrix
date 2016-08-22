using System;
using System.Collections.Generic;
using PythogorasSquare.WebApp.ViewModels.Qualities;

namespace PythogorasSquare.WebApp.ViewModels.Home
{
    public class PsychoMatrixViewModel
    {
        public DateTime DateOfBirth { get; }

        public IReadOnlyList<QualityViewModel> Qualities { get; }


        public PsychoMatrixViewModel(DateTime dateOfBirth, IReadOnlyList<QualityViewModel> qualities)
        {
            DateOfBirth = dateOfBirth;
            Qualities = qualities;
        }
    }
}