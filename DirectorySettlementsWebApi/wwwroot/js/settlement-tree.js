
// Settlements tree.
function SettlementsTree(actionsObject) {
    let treeNodes;
    // Builds tree of the settlement.
    this.buildTree = function(selector, nodes = []) {
        let tree = document.querySelector(selector);
        if (tree == null || tree == undefined)
            throw new Error('selector fails.');
        if (nodes.length == 0) throw new Error("empty");
        // clear tree.
        tree.innerHTML = "";
        // Main list of the nodes.
        let ul = document.createElement("ul");
        ul.classList.add("settlement-tree-root");
        tree.appendChild(ul);
        buildList(nodes, ul);
    }

    // Builds list of the node from one level.
    function buildList(nodes, ul) {
        treeNodes = nodes;
        for (let node of nodes) {
            let li = document.createElement("li");
            ul.appendChild(li);
            buildNode(node, li);
        }
    }

    // Builds settlement node.
    function buildNode(node, li) {
        if (node.nodes.length == 0) {
            //li.textContent = node.text;
            buildNodeTitle(node, li);
        }
        else {
            buildNodeWithChildren(node, li);
        }
    }

    // Builds node Title
    function buildNodeTitle(node, li) {
        let titleSpan = document.createElement("span");
        titleSpan.textContent = node.text;
        li.appendChild(titleSpan);

        let menuPanelSpan = document.createElement("span");
        menuPanelSpan.classList.add("menu-panel-span");
        li.appendChild(menuPanelSpan);

        addButton(menuPanelSpan, ["glyphicon", "glyphicon-plus"], onAddNode);
        addButton(menuPanelSpan, ["glyphicon", "glyphicon-pencil"], onEditNode);
        addButton(menuPanelSpan, ["glyphicon", "glyphicon-remove"], onDeleteNode);

        function onAddNode(event) {
            node.li = li;
            actionsObject.addNode(node);
        }

        function onEditNode(event) {
            node.li = li;
            actionsObject.editNode(node);
        }

        function onDeleteNode(event) {
            node.li = li;
            actionsObject.deleteNode(node);
        }
    }

    

    // Adds button to panel.
    function addButton(menuPanelSpan, cssClasses, onClickHandler) {
        let buttonSpan = document.createElement("span");
        for (let cssClass of cssClasses)
            buttonSpan.classList.add(cssClass);
        menuPanelSpan.appendChild(buttonSpan);
        buttonSpan.addEventListener("click", onClickHandler);
    }

    // Builds node which has children nodes.
    function buildNodeWithChildren(node, li) {
        let span = document.createElement("span");
        span.classList.add("caret");
        //span.textContent = node.text;
        li.appendChild(span);

        buildNodeTitle(node, li);

        let ul = document.createElement("ul");
        ul.classList.add("nested");
        li.appendChild(ul);

        span.addEventListener("click", expanderClick);

        // Opens children nodes.
        function expanderClick(event) {
            console.log('click expander');
            // Build children nodes if they opens at first time.
            if (ul.innerHTML == "")
                buildList(node.nodes, ul);
            // Show or hide children nodes.
            this.parentElement.querySelector(".nested").classList.toggle("active");
            this.classList.toggle("caret-down");
            event.stopPropagation();
        }
    }

    // Adds node to the tree.
    this.addNode = function (node, li) {
        li.innerHTML = "";
        buildNodeWithChildren(node, li);
    }

    // Edits node in the tree.
    this.editNode = function (node, li) {
        li.innerHTML = "";
        buildNode(node, li);
    }

    // Deletes node from the tree.
    this.deleteNode = function (node, li) {
        li.innerHTML = "";
        let parentUl = li.parentElement;
        parentUl.removeChild(li);
        console.log(node.te);
        let isRemoved = removeNode(node, treeNodes);
        console.log(isRemoved);
        let childNodes = parentUl.querySelectorAll("li");
        if (childNodes.length == 0) {
            let parentLiUl = parentUl.parentElement;
            parentLiUl.removeChild(parentUl);
            let caret = parentLiUl.querySelector("span.caret");
            parentLiUl.removeChild(caret);
        }
    }

    // Removes node from the tree.
    function removeNode(removedNode, nodes) {
        for (let i = 0; i < nodes.length; i++) {
            if (nodes[i].te == removedNode.te) {
                nodes.splice(i, 1);
                return true;
            }
            else {
                let isDeleted = removeNode(removedNode, nodes[i].nodes);
                if (isDeleted == true) return true;
            }
        }
        return false;
    }
}



