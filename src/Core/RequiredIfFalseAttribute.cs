﻿namespace NsaIpa.Web.Core;

public class RequiredIfFalseAttribute : RequiredIfAttribute
{
    public RequiredIfFalseAttribute(string dependentProperty) : base(dependentProperty, Operator.EqualTo, false) { }
}