from flask import Flask, jsonify, request

app = Flask(__name__)

# Mock database
items = [
    {"id": 1, "name": "Laptop", "price": 999.99},
    {"id": 2, "name": "Smartphone", "price": 499.99}
]

# READ: Get all items
@app.route('/api/items', methods=['GET'])
def get_items():
    return jsonify(items), 200

# READ: Get a single item by ID
@app.route('/api/items/<int:item_id>', methods=['GET'])
def get_item(item_id):
    item = next((i for i in items if i["id"] == item_id), None)
    if item is None:
        return jsonify({"error": "Item not found"}), 404
    return jsonify(item), 200

# CREATE: Add a new item
@app.route('/api/items', methods=['POST'])
def create_item():
    if not request.json or 'name' not in request.json or 'price' not in request.json:
        return jsonify({"error": "Bad Request. Name and price are required."}), 400
    
    new_item = {
        "id": items[-1]["id"] + 1 if items else 1,
        "name": request.json['name'],
        "price": request.json['price']
    }
    items.append(new_item)
    return jsonify(new_item), 201

# UPDATE: Modify an existing item
@app.route('/api/items/<int:item_id>', methods=['PUT'])
def update_item(item_id):
    item = next((i for i in items if i["id"] == item_id), None)
    if item is None:
        return jsonify({"error": "Item not found"}), 404
    
    if not request.json:
        return jsonify({"error": "Bad Request"}), 400

    item['name'] = request.json.get('name', item['name'])
    item['price'] = request.json.get('price', item['price'])
    return jsonify(item), 200

# DELETE: Remove an item
@app.route('/api/items/<int:item_id>', methods=['DELETE'])
def delete_item(item_id):
    global items
    item = next((i for i in items if i["id"] == item_id), None)
    if item is None:
        return jsonify({"error": "Item not found"}), 404
    
    items = [i for i in items if i["id"] != item_id]
    return jsonify({"message": "Item successfully deleted"}), 200

if __name__ == '__main__':
    app.run(debug=True)
