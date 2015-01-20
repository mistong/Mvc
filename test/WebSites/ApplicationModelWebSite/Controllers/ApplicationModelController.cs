// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ApplicationModels;
using Microsoft.AspNet.Mvc.ModelBinding;

namespace ApplicationModelWebSite
{
    // This controller uses an reflected model attribute to add arbitrary data to controller and action model.
    [ControllerDescription("Common Controller Description")]
    public class ApplicationModelController : Controller
    {
        public string GetCommonDescription()
        {
            var actionDescriptor = (ControllerActionDescriptor)ActionContext.ActionDescriptor;
            return actionDescriptor.Properties["description"].ToString();
        }

        [ActionDescription("Specific Action Description")]
        public string GetActionSpecificDescription()
        {
            var actionDescriptor = (ControllerActionDescriptor)ActionContext.ActionDescriptor;
            return actionDescriptor.Properties["description"].ToString();
        }

        private class ControllerDescriptionAttribute : Attribute, IControllerModelConvention
        {
            private object _value;

            public ControllerDescriptionAttribute(object value)
            {
                _value = value;
            }

            public void Apply(ControllerModel model)
            {
                model.Properties["description"] = _value;
            }
        }

        private class ActionDescriptionAttribute : Attribute, IActionModelConvention
        {
            private object _value;

            public ActionDescriptionAttribute(object value)
            {
                _value = value;
            }

            public void Apply(ActionModel model)
            {
                model.Properties["description"] = _value;
            }
        }
    }
}