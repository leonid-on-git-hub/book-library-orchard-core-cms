Ext.define('BookLibrary.UI.Books.validator.BookYearValidator', {
    extend: 'Ext.data.validator.Validator',
    alias: 'validator.bookyearvalidator',
    alternateClassName: 'BookYearValidator',
    singleton: true,
    validate: function (value){
        if(!value){
            return "The book year can't be empty";
        }
        if(Ext.Date.parse(value, Constants.DATE_FORAMAT) < Constants.MIN_DATE){
            return "The date in this field must be equal to or after 01/01/1753";
        }
        return true;
    }
});