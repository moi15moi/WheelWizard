using System;

namespace CT_MKWII_WPF.Models.Settings;

public abstract class Setting
{
    public Setting(Type type, string name, object defaultValue)
    {
        Name = name;
        DefaultValue = defaultValue;
        Value = defaultValue;
        ValueType = type;
    }
    
    public string Name { get; protected set; }
    public object DefaultValue { get; protected set; }
    protected object Value { get; set; }
    protected Func<object, bool>? ValidationFunc { get; set; }
    protected bool SaveEvenIfNotValid { get; set; }
    public Type ValueType { get; protected set; } 
 
    public abstract bool Set(object value, bool skipSave = false);
    public abstract object Get();
    public void Reset() => Set(DefaultValue);
    public abstract bool IsValid();
    
    public Setting SetValidation(Func<object?, bool> validationFunc)
    { 
        ValidationFunc = validationFunc;
        return this;
    }

    public Setting SetForceSave(bool saveEvenIfNotValid)
    {
        SaveEvenIfNotValid = saveEvenIfNotValid;
        return this;
    }
}
