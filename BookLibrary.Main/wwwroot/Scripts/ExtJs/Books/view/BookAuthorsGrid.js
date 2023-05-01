Ext.define('BookLibrary.UI.Books.view.BookAuthorsGrid', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.bookauthorsgrid',
    controller: 'bookauthorsgridcontroller',
    title: 'Authors',
    requires: [
        'Ext.window.MessageBox',
        'Ext.grid.plugin.CellEditing',
        'Ext.toolbar.Toolbar'
    ],
    hideHeaders: true,
    collapsible: false,
    viewConfig: {
        enableTextSelection: true,
        markDirty: false,
        listeners: {
            itemcontextmenu: 'onGridContextMenu'
        }
    },
    plugins: [{
        ptype: 'cellediting',
        clicksToEdit: 1
    }],
    columns: [{
        dataIndex: 'name',
        text: 'Author',
        flex: 1,
        editor: {
            xtype: 'textfield',
            bind: {
                editable: '{!readOnly}'
            },
            validator: function (value) {
                if (value.length > 64) {
                    return 'The author name max length is 64 characters';
                }
                return true;
            }
        }
    }],
    actions: {
        add: {
            text: 'Add',
            handler: 'onAdd'
        },
        delete: {
            text: 'Delete',
            handler: 'onDelete',
            bind: {
                disabled: '{authorsSelection == null}'
            }
        }
    },
    tbar: ['@add', '-', '@delete'],
    listeners: {
        afterrender: function (me) {
            var readOnly = me.up('window').getViewModel().get('readOnly');
            if (readOnly) {
                me.dockedItems.items.filter(item => item.id.includes('toolbar')).forEach(item => item.hide());
            }
        }
    }
});