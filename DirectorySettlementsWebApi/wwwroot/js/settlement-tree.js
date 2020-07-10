
// Builds tree of the settlement.
function buildTree(selector, nodes = []) {
    let tree = document.querySelector(selector);
    if (tree == null || tree == undefined) {
        console.log('selector fails.');
        return;
    }
    if (nodes.length == 0) {
        console.log("empty");
        return;
    }
    // Main list of the nodes.
    let ul = document.createElement("ul");
    ul.classList.add("settlement-tree-root");
    tree.appendChild(ul);
    buildList(nodes, ul);
}

// Builds list of the node from one level.
function buildList(nodes, ul) {
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