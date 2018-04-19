CSharpChain Blockchain
===========
Blockchain implementation written in C#. It consists of the definition of the blockchain, block and transaction. 
Communication between different nodes is implemented as selft hosted Web API. 
I wrote this project to deeply understand all aspects of distributed blockchain systems. 

Projects
------------

The solution consists of three projects:

**CSharpChainModel** with definition of data models for blockchain, block and transaction

**CSharpChainServer** code for running blockchain node locally

**CSharpChainNetwork** code for running blockchain in network mode (distributed)

How to run it
-------------
I would suggest you to build the project CSharpChainNetwork. The project was written with Visual Studio 2017. After the project is built,
go to the ./bin folder and start first node with the following parameters from the command line:

```
CSharpChainNetwork.exe http://localhost:8000
```

This will run first node on local machine on port 8000.

After the first node is up and running, you can start new one on different port. For example on 7000:

```
CSharpChainNetwork.exe http://localhost:7000
```

You can run as many nodes as you wish. 

Once the nodes are up and running you can start typing commands, like adding new transactions, check pending transactions, mine the blocks.
Until you connect two nodes you are operating on a local level. When you would like to connect nodes, use command **na**. 
In your 8000 instance, you can execute the command below to connect nodes running on ports 7000 and 8000. 
To list connected nodes, use **nl** command.

```
na http://localhost:7000
nl
```

The fun part starts when you add new transactions - for example when you transfer coins from one person to another person:

```
ta trumpAddress putinAddress 123 tx1
tp
```

**tp** lists all pending transaction on the local node (mempool).

Once you decide, you can create a block. This is the process of mining. In this sample I decided to use SHA-256 hashing
algorith with the difficulty of 4 (four leading zeroes). When mining a block, you need to include the address which will
receive mining award.

```
bm trumpAddress
```

After the block is mined, the notification is sent to the network of all nodes. Nodes then check for the longest blockchain and
synchronises with it. To manually update blockchain use **update** command.

```
update
```

Commands
--------
Below is a list of commands you can use once the node is up and running:

**h, help** = list of commands.

**q, quit** = exit the program.

**na, node-add [url]** = connect current node to other node.

**nr, node-remove [url]** = disconnect current node from other node.

**nl, nodes-list** = list connected nodes.

**ta, transaction-add [senderAddress] [receiverAddress] [Amount] [Description]** = create new transaction.

**tp, transactions-pending** = list of transactions not included in block

**bm, blockchain-mine [rewardAddress]** = create block from pending transactions

**bv, blockchain-valid** = Validates blockchain.

**bl, blockchain-length** = number of blocks in blockchain.

**b, block [index]** = list info about specified block.

**bu, update, blockchain-update** = update blockchain to the longest in network.

**bal, balance-get [address]** = get balance for specified address.

