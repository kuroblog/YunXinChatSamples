﻿using Desktop.Samples.Common;
using Desktop.Samples.Modules.Test.ViewModels;
using Microsoft.Practices.Prism.Logging;
using System;
using System.Windows.Controls;

namespace Desktop.Samples.Modules.Test.Views
{
    public partial class ControlTestView : UserControl
    {
        private readonly ILoggerFacade _logger;

        public ControlTestView(
            ControlTestViewModel viewModel,
            ILoggerFacade logger)
        {
            InitializeComponent();

            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug($"{GetType().Name} ... ctor.");
        }
    }
}
