Ext.define('BookLibrary.UI.Books.validator.BookNameValidator', {
    extend: 'Ext.data.validator.Validator',
    alias: 'validator.booknamevalidator',
    validate: function (value){
        if(value.match(/^ *$/) !== null){
            return "The book name can't be empty or whitespace";
        }
        if(value.length > 1024){
            return 'The book name max length is 1024 characters'
        }
        return true;
    }
});