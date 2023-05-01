Ext.define('BookLibrary.UI.Books.view.window.BookWindow', {
    extend: 'Ext.window.Window',
    xtype: 'bookwindow',
    alias: 'bookwindow',
    title: 'Book',
    requires: [
        'BookLibrary.UI.Books.validator.BookNameValidator',
        'BookLibrary.UI.Books.validator.BookYearValidator'
    ],

    controller: 'bookcontroller',
    viewModel: {
        type: 'bookviewmodel'
    },
    height: 700,
    width: 1200,
    modal: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    items: [{
        xtype: 'form',
        flex: 1,
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        items: [{
            xtype: 'textareafield',
            padding: '20 20 0 20',
            grow: true,
            msgTarget: 'under',
            minHeight: 70,
            growMax: 300,
            fieldLabel: 'Name',
            bind: {
                value: '{book.name}',
                readOnly: '{readOnly}'
            },
            validator: function (value) {
                var validator = Ext.create('BookLibrary.UI.Books.validator.BookNameValidator');
                return validator.validate(value);
            }
        }, {
            xtype: 'container',
            padding: '10 20 0 20',
            layout: {
                type: 'hbox'
            },
            items: [{
                xtype: 'datefield',
                fieldLabel: 'Year',
                format: Constants.DATE_FORAMAT,
                msgTarget: 'under',
                maxValue: new Date(),
                flex: 8,
                bind: {
                    value: '{book.year}',
                    readOnly: '{readOnly}'
                },
                validator: function (value) {
                    var validator = Ext.create('BookLibrary.UI.Books.validator.BookYearValidator');
                    return validator.validate(value);
                }
            }, {
                xtype: 'checkboxfield',
                fieldLabel: 'Available',
                reference: 'available-checkbox',
                labelAlign: 'left',
                labelWidth: 60,
                padding: '0 0 0 30',
                flex: 2,
                bind: {
                    value: '{book.available}',
                    readOnly: '{readOnly || bookAvailableReadOnly}'
                },
                listeners: {
                    afterrender: function (me, eOpts) {
                        me.originalValue = me.up('window').getViewModel().get('book.available');
                    }
                }
            }, {
                xtype: 'container',
                flex: 10
            }]
        }, {
            xtype: 'container',
            flex: 1,
            padding: '0 20 20 20',
            layout: {
                type: 'hbox',
                align: 'stretch'
            },
            items: [{
                xtype: 'container',
                flex: 1,
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'bookauthorsgrid',
                    flex: 1,
                    margin: '20 10 0 0',
                    bind: {
                        store: '{bookAuthorsStore}',
                        selection: '{authorsSelection}',
                        hidden: '{readOnly && book.authors.length == 0}'
                    }
                }]
            }, {
                xtype: 'container',
                flex: 1,
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'booktracksgrid',
                    margin: '20 0 0 10',
                    flex: 1,
                    bind: {
                        store: '{bookTracks}',
                        hidden: '{isNew}'
                    }
                }]
            }]
        },]
    }, {
        xtype: 'container',
        height: 50,
        layout: 'border',
        defaults: {
            xtype: 'button',
            width: 100,
            region: 'east',
            margin: 10
        },
        items: [{
            text: 'Cancel',
            handler: 'onClose',
            bind: {
                hidden: '{readOnly && !isNew }'
            }
        }, {
            text: 'Ok',
            handler: 'onClose',
            bind: {
                hidden: '{!readOnly || isNew }'
            }
        }, {
            text: 'Save',
            handler: 'onSave',
            bind: {
                hidden: '{readOnly || !isNew}'
            }
        }, {
            text: 'Save',
            handler: 'onEdit',
            bind: {
                hidden: '{readOnly || isNew}'
            }
        }]
    }]
});