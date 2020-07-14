// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Id of the tree div tag.
const treeId = "#tree";

let tree;
let selectedNodes = new Array();
let settlementsTree;

// Main function.
$(async function () {
    
    tree = await httpGetRootSettlements();
    settlementsTree = new SettlementsTree({
        addNode: createNodeModal,
        editNode: editNodeModal,
        deleteNode: deleteNodeModal
    });
    settlementsTree.buildTree(treeId, tree);
    addFilter();
    createNode();
    updateNode();
    deleteNode();
    exportTree();
});

// Adds filter.
function addFilter() {
    document.forms["filterForm"].addEventListener("submit", async e => {
        e.preventDefault();
        const form = document.forms["filterForm"];
        const name = form.elements["name"].value;
        const type = form.elements["type"].value;
        tree = await httpfilterNodes(name, type);
        settlementsTree.buildTree(treeId, tree);
    });
}

// Exports tree node.
function exportTree() {
    let exportButton = document.querySelector("#export");
    exportButton.addEventListener("click", async e => {
        e.preventDefault();
        const form = document.forms["filterForm"];
        const name = form.elements["name"].value;
        const type = form.elements["type"].value;
        await httpExportNodes(name, type);
    });
}

const createModalId = "#createModal";
const createFormName = "createForm";
// Opens create modal.
function createNodeModal(node) {
    $(createModalId).modal();
    const form = document.forms[createFormName];
    form.elements["te"].value = node.te;
    form.elements["name"].value = "";
    form.elements["type"].value = "";
    form.settlementParent = node;
}

const errorMessageBoxId = "#errorMessageBox";
// Creates a new node.
function createNode() {
    document.forms[createFormName].addEventListener("submit", async e => {
        e.preventDefault();
        const form = document.forms[createFormName];
        let node = {};
        node.te = form.elements["te"].value;
        node.nu = form.elements["name"].value;
        node.np = form.elements["type"].value;
        try {
            let createdNode = await httpCreateNode(node);
            let parent = form.settlementParent;
            parent.nodes.push(createdNode);

            $(createModalId).modal("toggle");
            settlementsTree.addNode(parent, parent.li);
        }
        catch (error) {
            $(createModalId).modal("toggle");            
            $(errorMessageBoxId).toast('show');
            $(`${errorMessageBoxId} div.errors`).text(error.message);
        }

    });
}

const editModalId = "#editModal";
const editFormName = "editForm";
// Opens edit modal.
function editNodeModal(node) {
    console.log(node.nu);
    $(editModalId).modal();
    const form = document.forms[editFormName];
    form.elements["name"].value = node.nu;
    form.elements["type"].value = node.np;
    form.settlement = node;
}

// Updates a node.
function updateNode() {
    document.forms[editFormName].addEventListener("submit", async e => {
        e.preventDefault();
        const form = document.forms[editFormName];
        let node = form.settlement;
        node.nu = form.elements["name"].value;
        node.np = form.elements["type"].value;
        try {
            let updatedNode = await httpUpdateNode(node);
            node.nu = updatedNode.nu;
            node.np = updatedNode.np;
            node.text = updatedNode.text;

            $(editModalId).modal("toggle");
            settlementsTree.editNode(node, node.li);
        }
        catch (error) {
            $(editModalId).modal("toggle");
            $(errorMessageBoxId).toast('show');
            $(`${errorMessageBoxId} div.errors`).text(error.message);
        }
    });
}

const deleteModalId = "#deleteModal";
const deleteFormName = "deleteForm";
// Opens delete modal.
function deleteNodeModal(node) {
    $(deleteModalId).modal();
    const form = document.forms[deleteFormName];
    form.elements["cascadeDelete"].checked = false;
    form.settlement = node;
}

// Delete a node.
function deleteNode() {
    document.forms[deleteFormName].addEventListener("submit", async e => {
        e.preventDefault();
        const form = document.forms[deleteFormName];
        let node = form.settlement;
        let isCascade = form.elements["cascadeDelete"].checked;
        console.log(isCascade);
        try {
            let deletedNode = await httpDeleteNode(node, isCascade);

            $(deleteModalId).modal("toggle");
            settlementsTree.deleteNode(node, node.li);
        }
        catch (error) {
            $(deleteModalId).modal("toggle");
            $(errorMessageBoxId).toast('show');
            $(`${errorMessageBoxId} div.errors`).text(error.message);
        }
    });
}