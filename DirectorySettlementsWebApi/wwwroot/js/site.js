// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const treeId = "#tree";
let selectedNodes = new Array();
let expandedNodes = [];
let tree = [];

$(async function () {
    
    tree = await getRootSettlements();
    buildTree(treeId, tree);
});

// Gets root settlements.
async function getRootSettlements() {
    const response = await fetch("/api/settlements", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        const settlements = await response.json();
        let nodes = getNodes(settlements);
        return nodes;
        //console.log(nodes);
        //createTree(nodes);
    }
}

// Converts Settlements to nodes.
function getNodes(settlements) {
    let nodes = [];
    // Mapping data.
    settlements.forEach(settlement => {
        let node = {};
        node.te = settlement.te;
        node.name = settlement.nu;
        node.text = `${node.name} [${node.te}]`;
        if (settlement.children)
            node.nodes = getNodes(settlement.children);
        nodes.push(node);
    });
    return nodes;
}
