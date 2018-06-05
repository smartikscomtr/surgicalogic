<template>
  <div>
    <v-card>
      <v-container grid-list-md>
        <v-card-title >
          <v-text-field v-model="search"
                        append-icon="search"
                        label="Search"
                        single-line
                        hide-details>
          </v-text-field>

            <v-btn icon
                  slot="activator"
                  class="mb-2">
              <v-icon color="teal">
                add
              </v-icon>
            </v-btn>
        </v-card-title>

        <v-data-table :columns="columns"
                      :items="items"
                      class="elevation-1">
          <template slot="items" slot-scope="props">
            <td v-for="(column, i) in columns" :key="i">
              {{ props.item[column.value] }}
            </td>

            <td slot="activator">
              <v-btn v-if="showEdit" icon class="mx-0" @click="editItem(props.item)">
                <v-icon color="teal">edit</v-icon>
              </v-btn>

              <v-btn v-if="showDelete" icon class="mx-0" @click="deleteItem(props.item)">
                <v-icon color="pink">delete</v-icon>
              </v-btn>
            </td>
            <template slot="no-data">
              <v-btn color="primary">Reset</v-btn>
            </template>
          </template>
        </v-data-table>
      </v-container>
    </v-card>
  </div>
</template>

<script>


export default {
  props: {
    items: {
      type: Array,
      required: false,
      default() {
        return [];
      }
    },

    columns: {
      type: Array,
      required: false,
      default() {
        return [];
      }
    },

    showEdit: {
      type: Boolean,
      required: false
    },

    showDelete: {
      type: Boolean,
      required: false
    }
  },

  data() {
    return {
      search: ''
    };
  },

  methods: {
    editItem(item) {
      const vm = this;

      vm.$emit('action', item);
    },

    deleteItem(item) {
      const vm = this;

      const index = vm.items.indexOf(item);

    confirm("Are you sure you want to delete this item?") &&
        this.items.splice(index, 1);
    }
  }
};
</script>
