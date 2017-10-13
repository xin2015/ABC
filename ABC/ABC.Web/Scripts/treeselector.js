function TreeSelector(selectorId, treeId, url, zNodes, chkStyle) {
    this.selectorId = selectorId;
    this.treeId = treeId;
    this.url = url;
    this.zNodes = zNodes;
    this.chkStyle = chkStyle;
    this.curExpandNode = null;
    this.selector = $("#" + selectorId);
    this.tree = $("#" + treeId);
    var treeSelector = this;
    this.setting = {
        check: {
            enable: true
        },
        view: {
            dblClickExpand: false,
            showIcon: false
        },
        data: {
            simpleData: {
                enable: true
            }
        },
        callback: {
            beforeExpand: function (treeId, treeNode) {
                treeSelector.beforeExpand(treeId, treeNode);
            },
            onExpand: function (event, treeId, treeNode) {
                treeSelector.onExpand(event, treeId, treeNode);
            },
            beforeClick: function (treeId, treeNode, clickFlag) {
                treeSelector.beforeClick(treeId, treeNode, clickFlag);
            },
            onCheck: function (event, treeId, treeNode) {
                treeSelector.onCheck(event, treeId, treeNode);
            }
        }
    };
}

TreeSelector.prototype.onAsyncSuccess = function (event, treeId, treeNode, msg) {
    var zTree = $.fn.zTree.getZTreeObj(treeId);
    var te = this.selector.val();
    var list = te.split(",");
    for (var i = 0; i < list.length; i++) {
        var node = zTree.getNodeByParam("name", list[i], null);
        if (node != null) {
            zTree.checkNode(node, true, false);
        }
    }
}

TreeSelector.prototype.beforeExpand = function (treeId, treeNode) {
    var pNode = this.curExpandNode ? this.curExpandNode.getParentNode() : null;
    var treeNodeP = treeNode.parentTId ? treeNode.getParentNode() : null;
    var zTree = $.fn.zTree.getZTreeObj(treeId);
    for (var i = 0, l = !treeNodeP ? 0 : treeNodeP.children.length; i < l; i++) {
        if (treeNode !== treeNodeP.children[i]) {
            zTree.expandNode(treeNodeP.children[i], false);
        }
    }
    while (pNode) {
        if (pNode === treeNode) {
            break;
        }
        pNode = pNode.getParentNode();
    }
    if (!pNode) {
        this.singlePath(treeNode);
    }
}

TreeSelector.prototype.singlePath = function (newNode) {
    if (newNode === this.curExpandNode) return;

    var zTree = $.fn.zTree.getZTreeObj(this.treeId),
            rootNodes, tmpRoot, tmpTId, i, j, n;

    if (!this.curExpandNode) {
        tmpRoot = newNode;
        while (tmpRoot) {
            tmpTId = tmpRoot.tId;
            tmpRoot = tmpRoot.getParentNode();
        }
        rootNodes = zTree.getNodes();
        for (i = 0, j = rootNodes.length; i < j; i++) {
            n = rootNodes[i];
            if (n.tId != tmpTId) {
                zTree.expandNode(n, false);
            }
        }
    } else if (this.curExpandNode && this.curExpandNode.open) {
        if (newNode.parentTId === this.curExpandNode.parentTId) {
            zTree.expandNode(this.curExpandNode, false);
        } else {
            var newParents = [];
            while (newNode) {
                newNode = newNode.getParentNode();
                if (newNode === this.curExpandNode) {
                    newParents = null;
                    break;
                } else if (newNode) {
                    newParents.push(newNode);
                }
            }
            if (newParents != null) {
                var oldNode = this.curExpandNode;
                var oldParents = [];
                while (oldNode) {
                    oldNode = oldNode.getParentNode();
                    if (oldNode) {
                        oldParents.push(oldNode);
                    }
                }
                if (newParents.length > 0) {
                    zTree.expandNode(oldParents[Math.abs(oldParents.length - newParents.length) - 1], false);
                } else {
                    zTree.expandNode(oldParents[oldParents.length - 1], false);
                }
            }
        }
    }
    this.curExpandNode = newNode;
}

TreeSelector.prototype.onExpand = function (event, treeId, treeNode) {
    this.curExpandNode = treeNode;
}

TreeSelector.prototype.beforeClick = function (treeId, treeNode, clickFlag) {
    var zTree = $.fn.zTree.getZTreeObj(treeId);
    zTree.checkNode(treeNode, !treeNode.checked, null, true);
    return false;
}

TreeSelector.prototype.onCheck = function (event, treeId, treeNode) {
    var zTree = $.fn.zTree.getZTreeObj(treeId),
    nodes = zTree.getCheckedNodes(true),
    v = "";
    for (var i = 0, l = nodes.length; i < l; i++) {
        v += nodes[i].name + ",";
    }
    if (v.length > 0) v = v.substring(0, v.length - 1);
    this.selector.attr("value", v);
};

TreeSelector.prototype.showMenu = function () {
    var selectorOffset = this.selector.offset();
    $("#menuContent").css({ left: selectorOffset.left + "px", top: selectorOffset.top + this.selector.outerHeight() + "px" }).slideDown("fast");
    var treeSelector = this;
    this.onBodyDownFunc = function (event) {
        treeSelector.onBodyDown(event);
    };
    $("body").unbind("mousedown", this.onBodyDownFunc).bind("mousedown", this.onBodyDownFunc);
}

TreeSelector.prototype.hideMenu = function () {
    $("#menuContent").fadeOut("fast");
    $("body").unbind("mousedown", this.onBodyDownFunc);
}

TreeSelector.prototype.onBodyDown = function (event) {
    if (!(event.target.id == this.selectorId || event.target.id == "menuContent" || $(event.target).parents("#menuContent").length > 0)) {
        this.hideMenu();
    }
}

TreeSelector.prototype.init = function () {
    if (this.chkStyle != undefined) {
        this.setting.check.chkStyle = this.chkStyle;
        this.setting.check.radioType = "all";
    } else {
        this.setting.check.chkboxType = { "Y": "", "N": "" };
    }
    if (this.url == undefined || this.url == null) {
        if (this.zNodes == undefined || this.zNodes == null) {
            alert("请配置url或者zNodes！");
        } else {
            $.fn.zTree.init(this.tree, this.setting, this.zNodes);
            this.onAsyncSuccess(null, this.treeId);
        }
    } else {
        this.setting.callback.onAsyncSuccess = this.onAsyncSuccess;
        $.fn.zTree.init(this.tree, this.setting);
    }

    var treeSelector = this;
    this.selector.click(function () {
        treeSelector.showMenu();
    });
}