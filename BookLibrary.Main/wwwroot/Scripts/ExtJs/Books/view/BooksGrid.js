Ext.define('BookLibrary.UI.Books.view.BooksGrid', {
    extend: 'Ext.grid.Panel',
    xtype: 'booksgrid',
    requires: [
        'Ext.window.MessageBox'
    ],
    renderTo: 'books-grid-container',
    controller: 'booksgridcontroller',
    viewModel: 'booksviewmodel',
    title: 'Books',
    bind: {
        store: '{booksPagedStore}',
        selection: '{booksGridSelection}'
    },
    collapsible: false,
    headerBorders: false,
    viewConfig: {
        enableTextSelection: true,
        listeners: {
            itemcontextmenu: 'onGridContextMenu',
            selectionChange: 'onSelectionChange'
        }
    },
    columns: [{
        text: 'Id',
        flex: 1,
        dataIndex: 'id',
        hidden: true
    }, {
        text: 'Name',
        flex: 4,
        dataIndex: 'name',
    }, {
        text: 'Authors',
        flex: 2,
        dataIndex: 'authors',
        renderer: function (value) {
            if (value) {
                return value.join(', ');
            } else {
                return '';
            }
        }
    }, {
        text: 'Year',
        flex: 1,
        dataIndex: 'year',
        renderer: function (value) {
            var date = new Date(value);
            const yyyy = date.getFullYear();
            let mm = date.getMonth() + 1;
            let dd = date.getDate();

            if (dd < 10) dd = '0' + dd;
            if (mm < 10) mm = '0' + mm;

            return dd + '/' + mm + '/' + yyyy;
        }
    }, {
        text: 'Available',
        flex: 1,
        dataIndex: 'available',
        renderer: function (value) {
            if (value) {
                return 'Available';
            } else {
                return 'Not available';
            }
        }
    }, {
        xtype: 'actioncolumn',
        width: 150,
        items: [{
            iconCls: 'fa fa-file-alt',
            tooltip: 'View',
            handler: 'onView'
        }, '-', {
            iconCls: 'fas fa-file-signature',
            tooltip: 'Edit',
            handler: 'onEdit'
        }, '-', {
            iconCls: 'x-fa fa-trash fa-trash-default',
            tooltip: 'Delete',
            handler: 'onDelete'
        }, '-']
    }],
    tbar: [
        '@add',
        '->', {
            xtype: 'textfield',
            fieldLabel: 'Name',
            labelWidth: 40,
            enableKeyEvents: true,
            bind: '{serachBookName}',
            listeners: {
                keypress: function (me, e, eOpts) {
                    if (e.getKey() == e.ENTER) {
                        me.up('grid').getController().onSearch();
                    }
                }
            }
        }, {
            xtype: 'textfield',
            fieldLabel: 'Author',
            labelWidth: 44,
            enableKeyEvents: true,
            bind: '{serachAuthor}',
            listeners: {
                keypress: function (me, e, eOpts) {
                    if (e.getKey() == e.ENTER) {
                        me.up('grid').getController().onSearch();
                    }
                }
            }
        }, {
            xtype: 'datefield',
            fieldLabel: 'Year',
            labelWidth: 30,
            format: 'Y',
            maxValue: new Date(),
            bind: '{serachYear}'
        }, {
            xtype: 'checkboxfield',
            labelAlign: 'left',
            labelWidth: 90,
            fieldLabel: 'Only available',
            bind: '{searchOnlyAvailable}'
        },
        '@clearsearch',
        '@search'],
    bbar: [{
        xtype: 'pagingtoolbar',
        bind: {
            store: '{booksPagedStore}'
        },
        displayInfo: true,
        displayMsg: '{0} to {1} of {2} &nbsp;books',
        emptyMsg: 'No books&nbsp;'
    }],
    actions: {
        add: {
            text: 'Add',
            disabled: false,
            handler: 'onAdd'
        },
        view: {
            text: 'View',
            bind: {
                disabled: '{booksGridSelection == null}'
            },
            handler: 'onView'
        },
        edit: {
            text: 'Edit',
            bind: {
                disabled: '{booksGridSelection == null}'
            },
            handler: 'onEdit'
        },
        delete: {
            text: 'Delete',
            bind: {
                disabled: '{booksGridSelection == null}'
            },
            handler: 'onDelete'
        },
        clearsearch: {
            text: 'Clear',
            handler: 'onClearSearch'
        },
        search: {
            text: 'Search',
            handler: 'onSearch'
        }
    },
    listeners: {
        itemdblclick: function () {
            this.controller.onView()
        }
    }
});