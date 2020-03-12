        internal void InsertTitleBlock(string titleBlockName, Point3d insPt, string[] titleBlockTags, string[] titleBlockArgs)
        {
            // Get Current Document and Database //
            Document doc = AcAp.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                /* Open BlockTable containing all BlockTableRecords
                 * ------------------------------------------------
                 * Block definitions are stored insit BlockTableRecords and if that block has 
                 * attribute definitions, then those are stored as AttributeDefinitions inside
                 * the BlockTableRecord.
                 * 
                 * Non-constant AttributeDefinition Case
                 * -------------------------------------
                 * A block is inserted as a BlockReference inside the drawing and for blocks with 
                 * attributes, the BlockReference has a AttiributeReference that a matching 
                 * AttributeDefinition in the BlockTableRecord. 
                 * 
                 */                    
                // Open BlockTable containing all BlockTableRecords
                BlockTable blkTbl = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                // Get block definition //
                BlockTableRecord blkDef = blkTbl[titleBlockName].GetObject(OpenMode.ForRead) as BlockTableRecord;

                // Get Current space
                BlockTableRecord currSpaceBlkTblRec = tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;

                using (BlockReference blkRef = new BlockReference(insPt, blkDef.ObjectId))
                {

                    currSpaceBlkTblRec.AppendEntity(blkRef);
                    tr.AddNewlyCreatedDBObject(blkRef, true);

                    /* ----------------------------------------------------------------------------
                     * IMPORTANT NOTE 
                     * --------------
                     * The title_block is inserted as a block in which the attribute information 
                     * cannot be edited (for some purpose). This attributes of this inserted block 
                     * needs to be "transformed" inside AttributeReference. 
                     * ----------------------------------------------------------------------------
                     */


                    foreach (ObjectId objID in blkDef)
                    {
                        DBObject dbObj = objID.GetObject(OpenMode.ForRead);
                        AttributeDefinition attDef = dbObj as AttributeDefinition;

                        if((attDef != null ) && (!attDef.Constant))
                        {
                            using(AttributeReference attRef = new AttributeReference())
                            {
                                attRef.SetAttributeFromBlock(attDef, blkRef.BlockTransform);
                                for(int i = 0; i < titleBlockTags.Length; i++)
                                {
                                    if (attRef.Tag == titleBlockTags[i]) attRef.TextString = titleBlockArgs[i];
                                }

                                blkRef.AttributeCollection.AppendAttribute(attRef);
                                tr.AddNewlyCreatedDBObject(attRef, true);
                            }
                        }
                    }


                }
                tr.Commit();
            }

        }
