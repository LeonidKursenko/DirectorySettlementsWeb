// Gets root settlements.
async function httpGetRootSettlements() {
    const response = await fetch("/api/settlements", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        const settlements = await response.json();
        let nodes = getNodes(settlements);
        return nodes;
        //console.log(nodes);
    }
}

// Converts Settlements to nodes.
function getNodes(settlements) {
    let nodes = [];
    // Mapping data.
    settlements.forEach(settlement => {
        //let node = {};
        //node.te = settlement.te;
        //node.name = settlement.nu;
        //node.text = `${node.name} [${node.te}]`;
        //if (settlement.children)
        //    node.nodes = getNodes(settlement.children);
        //nodes.push(node);
        let node = getNode(settlement);
        nodes.push(node);
    });
    return nodes;
}

// Converts Settlement to node.
function getNode(settlement) {
    // Mapping data.
    let node = {};
    node.te = settlement.te;
    node.nu = settlement.nu;
    node.text = `${node.nu} [${node.te}]`;
    if (settlement.children)
        node.nodes = getNodes(settlement.children);
    return node;
}

// Gets filtered settlements.
async function httpfilterNodes(searchedName, searchedType) {
    const response = await fetch(`api/settlements/filter?name=${searchedName}&type=${searchedType}`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        const settlements = await response.json();
        let nodes = getNodes(settlements);
        return nodes;
        //buildTree(treeId, nodes);
    }
}

// Creates a new node.
async function httpCreateNode(node) {
    console.log("Node is created");
    const response = await fetch("api/settlements", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            te: node.te,
            nu: node.nu,
            np: node.np
        })
    });
    if (response.ok === true) {
        const settlement = await response.json();
        return getNode(settlement);
    }
}

// Updates a node.
async function httpUpdateNode(node) {
    console.log("Node is updated");
    const response = await fetch("api/settlements/" + node.te, {
        method: "PUT",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            te: node.te,
            nu: node.nu,
            np: node.np
        })
    });
    if (response.ok === true) {
        const settlement = await response.json();
        return getNode(settlement);
    }
}

// Delete a node.
async function httpDeleteNode(node, isCascade) {
    const response = await fetch(`api/settlements/delete?te=${node.te}&cascade=${isCascade}`, {
        method: "DELETE",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        const settlement = await response.json();
        return getNode(settlement);
    }
}
