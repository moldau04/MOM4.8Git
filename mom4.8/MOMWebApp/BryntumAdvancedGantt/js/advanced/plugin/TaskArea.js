Ext.define('Gnt.examples.advanced.plugin.TaskArea', {
    extend  : 'Ext.AbstractPlugin',
    alias   : 'plugin.taskarea',

    tpl     : '<div class="gnt-taskarea" style="{height}px"><div class="gnt-taskarea-inner"></div></div>',

    config  : {
        enabled : false
    },

    setEnabled : function () {
        this.callParent(arguments);

        if (this.panel) this.repaintAllAreas();
    },

    init : function (panel) {
        var me = this,
            bodyTpl = (panel.parentTaskBodyTemplate || Gnt.template.ParentTask.prototype.innerTpl) + me.tpl;

        Ext.apply(panel.getSchedulingView(), {
            parentTaskBodyTemplate : bodyTpl
        });

        me.panel = panel;
        me.taskStore = panel.getTaskStore();

        panel.getSchedulingView().on({
            refresh    : this.repaintAllAreas,

            // These are also fired on expand / collapse
            itemupdate : this.repaintAllAreas,
            itemremove : this.repaintAllAreas,
            itemadd    : this.repaintAllAreas,
            delay      : 1, // Allow view to finish painting rows first
            scope      : this
        });

        panel.on('afterrender', function() {
            me.getEnabled() && panel.getSchedulingView().getEl().addCls('gnt-taskarea-enabled');
        }, null, {single : true});
    },

    getLastVisibleChild : function (parentNode) {
        var result;

        if (!parentNode || parentNode.isLeaf() || !parentNode.isExpanded()) {
            result = parentNode;
        }
        else {
            result = this.getLastVisibleChild(parentNode.lastChild);
        }

        return result;
    },

    repaintAllAreas : function () {

        if (!this.getEnabled()) return;

        var view = this.panel.getSchedulingView();
        var viewNodes = this.panel.getSchedulingView().getNodes();

        Ext.Array.each(viewNodes, function (domNode) {
            if (domNode) {
                var node = view.getRecord(domNode);

                if (node && !node.isRoot() && !node.isLeaf() && !node.isMilestone() && node.isExpanded()) {
                    this.refreshAreaSize(node);
                }
            }
        }, this);
    },

    refreshAreaSize : function (parentTask) {

        var view = this.panel.getSchedulingView(),
            elements = view.getElementsFromEventRecord(parentTask),
            el = elements && elements[0];

        if (!el) return;

        var areaEl = el.down('.gnt-taskarea');

        if (!areaEl) return;

        var lastVisibleChild = this.getLastVisibleChild(parentTask);
        var height = 0;

        if (lastVisibleChild && lastVisibleChild !== parentTask) {
            var lastNode = view.getNode(lastVisibleChild);
            if (lastNode) {
                height = Ext.fly(lastNode).getY() - areaEl.getY() + Ext.fly(lastNode).getHeight();
            }
        }

        // we should filter height less than 1 for IE8
        if (height < 0) {
            return;
        }

        areaEl.setHeight(height);
    },

    updateEnabled : function (value, oldValue) {
        var cmp = this.getCmp(),
            view = cmp.rendered && cmp.getSchedulingView();

        if (view) {
            if (value) {
                view.getEl().addCls('gnt-taskarea-enabled');
            } else {
                view.getEl().removeCls('gnt-taskarea-enabled');
            }
        }
    }
});
