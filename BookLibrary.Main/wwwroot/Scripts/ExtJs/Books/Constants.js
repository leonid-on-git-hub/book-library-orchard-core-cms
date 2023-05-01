Ext.define('BookLibrary.UI.Books.model.Constants', {
    singleton: true,
    alternateClassName: 'Constants',
    APPLICATION_BASE_URL: typeof appBaseUrl !== 'undefined' ? appBaseUrl : '',
    DATE_FORAMAT: 'd/m/Y',
    MIN_DATE: Ext.Date.parse('01/01/1753', this.DATE_FORAMAT),

    getRequestVerificationToken: function(){
        var input = Ext.select('#anti-forgery input').item(0);
        if(input){
            return input.getValue();
        } else {
            return '';
        }
    }
});